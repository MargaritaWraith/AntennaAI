using System;
using System.Collections.Generic;
using AntennaAI.AI.NeuralNetworks.ActivationFunctions;
using AntennaAI.AI.NeuralNetworks.Interfaces;

namespace AntennaAI.AI.NeuralNetworks
{
    /// <summary>Многослойный персептрон</summary>
    public partial class MultilayerPerceptron : ITeachableNeuralNetwork
    {
        /* --------------------------------------------------------------------------------------------- */

        #region Поля 

        /// <summary>Матрицы коэффициентов передачи слоёв (номер строки - номер нейрона; номер столбца - номер входа нейрона; последний столбец - смещение нейрона)</summary>
        protected readonly double[][,] _Layers;

        /// <summary>Массив смещений нейронов в слоях (первый индекс - номер слоя; второй - номер нейрона в слое)</summary>
        protected readonly double[][] _Offsets;

        /// <summary>Массив весовых коэффициентов смещений нейронов в слоях (первый индекс - номер слоя; второй - номер нейрона в слое)</summary>
        protected readonly double[][] _OffsetsWeights;

        /// <summary>Массив выходов скрытых слоёв</summary>
        protected readonly double[][] _Outputs;

        /// <summary>Функции активации слоёв</summary>
        protected readonly ActivationFunction[] _Activations;

        #endregion

        /* --------------------------------------------------------------------------------------------- */

        #region Свойства

        /// <summary>Слой</summary>
        public LayersManager Layer => new LayersManager(_Layers, _Offsets, _OffsetsWeights, _Outputs, _Activations);

        /// <summary>Входной слой</summary>
        public LayerManager LayerInput => Layer[0];

        /// <summary>Выходной слой</summary>
        public LayerManager LayerOutput => Layer[LayersCount - 1];

        public int InputsCount => _Layers[0].GetLength(1);

        public int OutputsCount => _Layers[_Layers.Length - 1].GetLength(0);

        /// <summary>Число слоёв</summary>
        public int LayersCount => _Layers.Length;

        /// <summary>Скрытые выходы</summary>
        public IReadOnlyList<double[]> HiddenOutputs => _Outputs;

        /// <summary>Индекс матриц весовых коэффициентов слоёв</summary>
        /// <param name="layer">Номер слоя</param>
        /// <returns>Матрица весовых коэффициентов слоёв</returns>
        public ref readonly double[,] this[int layer] => ref _Layers[layer];

        /// <summary>Смещения слоёв</summary>
        public IReadOnlyList<double[]> Offests => _Offsets;

        /// <summary>Весовые коэффициенты смещений слоёв</summary>
        public IReadOnlyList<double[]> OffsetWeights => _OffsetsWeights;

        #endregion

        /* --------------------------------------------------------------------------------------------- */

        #region Конструкторы

        /// <summary>Инициализация новой многослойной нейронной сети</summary>
        /// <param name="Layers">Набор матриц коэффициентов передачи слоёв</param>
        public MultilayerPerceptron(params double[][,] Layers)
        {
            _Layers = Layers ?? throw new ArgumentNullException(nameof(Layers));
            if (Layers.Length == 0) throw new ArgumentException("Число слоёв должно быть больше 0", nameof(Layers));

            var layers_count = Layers.Length;
            _Outputs = new double[layers_count - 1][];
            _Offsets = new double[layers_count][];
            _OffsetsWeights = new double[layers_count][];

            _Activations = new ActivationFunction[layers_count];

            // Создаём структуры слоёв
            for (var layer_index = 0; layer_index < layers_count; layer_index++)
            {
                // Количество входов слоя
                var inputs_count = _Layers[layer_index].GetLength(1);

                // Проверяем - если слой не первый и количество выходов предыдущего слоя не совпадает с количеством входов текущего слоя, то это ошибка структуры сети
                if (layer_index > 0 && _Layers[layer_index - 1].GetLength(0) != inputs_count)
                    throw new FormatException($"Количество входов слоя {layer_index} не равно количеству выходов слоя {layer_index - 1}");

                //Количество выходов слоя (количество нейронов)
                var outputs_count = _Layers[layer_index].GetLength(0);

                if (layer_index < layers_count - 1)
                    _Outputs[layer_index] = new double[outputs_count];    // Выходы слоя

                var offsets = new double[outputs_count];
                var offsets_weights = new double[outputs_count];
                for (var i = 0; i < outputs_count; i++)
                {
                    offsets[i] = 1;
                    offsets_weights[i] = 1;
                }
                _Offsets[layer_index] = offsets;                // Создаём массив смещений нейронов слоя и инициализируем его единицами
                _OffsetsWeights[layer_index] = offsets_weights; // Создаём массив коэффициентов смещений для слоя и инициализируем его единицами
            }
        }

        #endregion

        /* --------------------------------------------------------------------------------------------- */

        #region Методы

        /// <summary>Обработка данных сетью</summary>
        /// <param name="Input">Массив входа</param>
        /// <param name="Output">Массив выхода</param>
        public virtual void Process(Span<double> Input, Span<double> Output) => Process(Input, Output, _Layers, _Activations, _Offsets, _OffsetsWeights, null, _Outputs);

        /// <summary>Обработка данных сетью</summary>
        /// <param name="Input">Массив входа</param>
        /// <param name="Output">Массив выхода</param>
        /// <param name="Layers">Массив матриц коэффициентов передачи слоёв</param>
        /// <param name="Activations">Массив активационных функций слоёв (если функция не задана, используется Сигмоид)</param>
        /// <param name="Offsets">Массив векторов смещений</param>
        /// <param name="OffsetsWeights">Массив векторов весовых коэффициентов смещений</param>
        /// <param name="State">
        /// Массив состояний (входов функций активации) слоёв.
        /// Может применяться в процессе обучения сети.
        /// Достаточно создать массив длиной, равной количеству слоёв с пустыми элементами.
        /// </param>
        /// <param name="Outputs">Массив векторов выходных значений слоёв</param>
        private static void Process(
            Span<double> Input,
            Span<double> Output,
            double[][,] Layers,
            ActivationFunction[] Activations,
            double[][] Offsets,
            double[][] OffsetsWeights,
            double[][] State,
            double[][] Outputs
        )
        {
            throw new NotImplementedException();
        }

        #endregion

            /* --------------------------------------------------------------------------------------------- */
        }
}
