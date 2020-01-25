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

        public MultilayerPerceptron() { }

        #endregion

        /* --------------------------------------------------------------------------------------------- */

        #region Методы
        
        public void Process(Span<double> Input, Span<double> Output) => throw new NotImplementedException(); 

        #endregion

        /* --------------------------------------------------------------------------------------------- */
    }
}
