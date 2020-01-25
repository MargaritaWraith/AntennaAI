using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>Инициализация матрицы весовых коэффициентов слоя</summary>
        /// <param name="LayerWeights">Матрица весовых коэффициентов слоя</param>
        /// <param name="LayerIndex">Индекс слоя</param>
        /// <param name="Initializer">Функция инициализации весовых коэффициентов слоя</param>
        private static void InitializeLayerWeightsMatrix(
            double[,] LayerWeights,
            int LayerIndex,
            NetworkCoefficientInitializer Initializer)
        {
            for (var i = 0; i < LayerWeights.GetLength(0); i++)
                for (var j = 0; j < LayerWeights.GetLength(1); j++)
                    LayerWeights[i, j] = Initializer.Invoke(LayerIndex, i, j);
        }

        /// <summary>Создать массив матриц передачи слоёв</summary>
        /// <param name="InputsCount">Количество входов сети</param>
        /// <param name="NeuronsCount">Количество нейронов в слоях</param>
        /// <param name="Initialize">Функция инициализации весовых коэффициентов</param>
        /// <returns>Массив матриц коэффициентов передачи слоёв сети</returns>
        private static double[][,] CreateLayersMatrix(
            int InputsCount,
            IEnumerable<int> NeuronsCount,
            NetworkCoefficientInitializer Initialize)
        {
            var neurons_count = NeuronsCount.ToArray();
            var layers_count = neurons_count.Length;
            var weights = new double[layers_count][,];

            var w = new double[neurons_count[0], InputsCount];
            weights[0] = w;
            InitializeLayerWeightsMatrix(w, 0, Initialize);

            for (var layer = 1; layer < layers_count; layer++)
            {
                w = new double[neurons_count[layer], neurons_count[layer - 1]];
                weights[layer] = w;
                InitializeLayerWeightsMatrix(w, layer, Initialize);
            }

            return weights;
        }

        /// <summary>Инициализатор нейронной связи</summary>
        /// <param name="Layer">Номер слоя</param>
        /// <param name="Neuron">Номер нейрона в слое</param>
        /// <param name="Input">Номер входа нейрона</param>
        /// <returns>Коэффициент передачи входа нейрона</returns>
        public delegate double NetworkCoefficientInitializer(int Layer, int Neuron, int Input);

        /// <summary>Инициализация новой многослойной нейронной сети</summary>
        /// <param name="InputsCount">Количество входов сети</param>
        /// <param name="NeuronsCount">Количество нейронов в слоях</param>
        /// <param name="rnd">Генератор случайных чисел для заполнения матриц коэффициентов передачи слоёв</param>
        public MultilayerPerceptron(
            int InputsCount,
            IEnumerable<int> NeuronsCount,
            Random rnd)
            : this(CreateLayersMatrix(InputsCount, NeuronsCount, (L, N, I) => rnd.NextDouble() - 0.5)) { }

        private static NetworkCoefficientInitializer GetStandardRandomInitializer()
        {
            var rnd = new Random();
            return (l, n, i) => rnd.NextDouble() - 0.5;
        }

        /// <summary>Инициализация новой многослойной нейронной сети</summary>
        /// <param name="InputsCount">Количество входов сети</param>
        /// <param name="NeuronsCount">Количество нейронов в слоях</param>
        /// <param name="Initialize">Функция инициализации коэффициентов матриц передачи слоёв</param>
        public MultilayerPerceptron(
            int InputsCount,
            IEnumerable<int> NeuronsCount,
            NetworkCoefficientInitializer Initialize = null)
            : this(CreateLayersMatrix(InputsCount, NeuronsCount, Initialize ?? GetStandardRandomInitializer())) { }

        /// <summary>Инициализатор слоя</summary><param name="Layer">Менеджер инициализируемого слоя</param>
        public delegate void LayerInitializer(LayerManager Layer);

        /// <summary>Инициализация новой многослойной нейронной сети</summary>
        /// <param name="InputsCount">Количество входов сети</param>
        /// <param name="NeuronsCount">Количество нейронов в слоях</param>
        /// <param name="Initializer">Функция инициализации слоёв сети</param>
        public MultilayerPerceptron(
            int InputsCount,
            IEnumerable<int> NeuronsCount,
            LayerInitializer Initializer)
            : this(InputsCount, NeuronsCount)
        {
            if (Initializer is null) return;
            foreach (var layer in Layer)
                Initializer(layer);
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
            if (Layers is null) throw new ArgumentNullException(nameof(Layers));
            if (Activations is null) throw new ArgumentNullException(nameof(Activations));
            if (Offsets is null) throw new ArgumentNullException(nameof(Offsets));
            if (OffsetsWeights is null) throw new ArgumentNullException(nameof(OffsetsWeights));
            if (Outputs is null) throw new ArgumentNullException(nameof(Outputs));

            if (Input.Length != Layers[0].GetLength(1)) throw new ArgumentException($"Размер входного вектора ({Input.Length}) не равен количествоу входов сети ({Layers[0].GetLength(1)})", nameof(Input));
            if (Output.Length != Layers[Layers.Length - 1].GetLength(0)) throw new ArgumentException($"Размер выходного вектора ({Output.Length}) не соответвтует количеству выходов сети ({Layers[Layers.Length - 1].GetLength(0)})", nameof(Output));
            if (Activations.Length != Layers.Length) throw new InvalidOperationException("Размер массива функций активации не соответствует количеству слоёв сети");

            var layer = Layers;                                     // Матрицы коэффициентов передачи слоёв
            var layers_count = layer.Length;                        // Количество слоёв
            var layer_activation = Activations;                     // Активационные функции слоёв
            var layer_offsets = Offsets;                            // Смещения слоёв
            var layer_offset_weights = OffsetsWeights;              // Весовые коэффициенты весов слоёв <= 0

            var state = State;
            var outputs = Outputs;

            for (var layer_index = 0; layer_index < layers_count; layer_index++)
            {
                // Определяем матрицу слоя W
                var current_layer_weights = layer[layer_index];
                // Определяем вектор входа X
                var prev_layer_output = layer_index == 0                 // Если слой первый, то за выходы "предыдущего слоя"
                    ? Input                                              // принимаем входной вектор
                    : outputs[layer_index - 1];                          // иначе берём массив выходов предыдущего слоя

                // Определяем вектор входа следующего слоя X_next         
                var current_output = layer_index == layers_count - 1     // Если слой последний, то за выходы "следующего слоя"
                    ? Output                                             // Принимаем массив выходного вектора
                    : outputs[layer_index]                               // иначе берём массив текущего слоя
                      ?? new double[current_layer_weights.GetLength(0)]; // Если выходного вектора нет, то создаём его!

                // Определяем вектор входа функции активации Net
                double[] current_state = null;
                if (state != null) current_state = state[layer_index] ?? new double[current_output.Length];

                // Определяем вектор смещения O (Offset)
                var current_layer_offset = layer_offsets[layer_index];
                // Определяем вектор весов смещения Wo (Weight of offset)
                var current_layer_offset_weights = layer_offset_weights[layer_index];

                var current_layer_activation = layer_activation[layer_index];

                ProcessLayer(
                    current_layer_weights,
                    current_layer_offset,
                    current_layer_offset_weights,
                    prev_layer_output,
                    current_output, current_layer_activation, current_state);
            }
        }

        /// <summary>Метод обработки одного слоя</summary>
        /// <param name="LayerWeights">Матрица коэффициентов передачи</param>
        /// <param name="Offset">Вектор смещений нейронов</param>
        /// <param name="OffsetWeight">Вектор весовых коэффициентов</param>
        /// <param name="Input">Вектор входного воздействия</param>
        /// <param name="Output">Вектор выходных значений нейронов</param>
        /// <param name="Activation">Активационная функция слоя (если не задана, то используется Сигмоид)</param>
        /// <param name="State">Вектор входа функции активации</param>
        private static void ProcessLayer(
            double[,] LayerWeights,
            double[] Offset,
            double[] OffsetWeight,
            Span<double> Input,
            Span<double> Output,
            ActivationFunction Activation = null,
            double[] State = null)
        {
            // Вычисляем X_next = f(Net = W * X + Wo*O)
            var layer_outputs_count = LayerWeights.GetLength(0);
            var layer_inputs_count = LayerWeights.GetLength(1);

            for (var output_index = 0; output_index < layer_outputs_count; output_index++)
            {
                var output = Offset[output_index] * OffsetWeight[output_index];
                for (var input_index = 0; input_index < layer_inputs_count; input_index++)
                    output += LayerWeights[output_index, input_index] * Input[input_index];
                if (State != null) State[output_index] = output;
                Output[output_index] = Activation?.Value(output) ?? Sigmoid.Activation(output);
            }
        }

        #endregion

        /* --------------------------------------------------------------------------------------------- */
    }
}
