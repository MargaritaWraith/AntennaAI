using System;
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

        public int InputsCount => _Layers[0].GetLength(1);

        public int OutputsCount => _Layers[_Layers.Length - 1].GetLength(0);

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
