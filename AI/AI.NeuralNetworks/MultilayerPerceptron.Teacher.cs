using System;
using System.Collections.Generic;
using System.Text;
using AntennaAI.AI.NeuralNetworks.Interfaces;

namespace AntennaAI.AI.NeuralNetworks
{
    public partial class MultilayerPerceptron
    {
        public INetworkTeacher CreateTeacher() => throw new NotImplementedException();

        public TNetworkTeacher CreateTeacher<TNetworkTeacher>(Action<TNetworkTeacher> Configurator = null) where TNetworkTeacher : class, INetworkTeacher => throw new NotImplementedException();

        private class BackPropagationTeacher : NetworkTeacher, IBackPropagationTeacher
        {
            /// <summary>Обучаемая сеть</summary>
            private readonly MultilayerPerceptron _Network;

            /// <summary>Ошибки на выходах нейронов в слоях</summary>
            private readonly double[][] _Errors;

            /// <summary>Состояние входов нейронов (аргументы активационной функции)</summary>
            private readonly double[][] _State;

            /// <summary>Величина изменения веса связи нейрона с предыдущей итерации обучения</summary>
            private readonly double[][,] _DW;

            /// <summary>Величина изменения веса смещения нейрона с предыдущей итерации обучения</summary>
            private readonly double[][] _DWoffset;

            /// <summary>Предыдущая ошибка прямого распространения</summary>
            private double _LastError = double.PositiveInfinity;

            /// <summary>Лучший вариант весов входов в слоях</summary>
            private readonly double[][,] _BestVariantW;

            /// <summary>Лучший вариант весов смещений нейронов в слоях</summary>
            private readonly double[][] _BestVariantOffsetW;

            public double Rho { get; set; } = 0.2;

            public double InertialFactor { get; set; }

            public BackPropagationTeacher(MultilayerPerceptron Network) : base(Network)
            {
                _Network = Network;
                var layers_count = _Network.LayersCount;
                _Errors = new double[layers_count][];
                _State = new double[layers_count][];
                _DW = new double[layers_count][,];
                _DWoffset = new double[layers_count][];
                _BestVariantW = new double[layers_count][,];
                _BestVariantOffsetW = new double[layers_count][];
                for (var i = 0; i < layers_count; i++)
                {
                    var (neurons_count, inputs_count) = _Network._Layers[i];
                    _Errors[i] = new double[neurons_count];
                    _State[i] = new double[neurons_count];
                    _DW[i] = new double[neurons_count, inputs_count];
                    _DWoffset[i] = new double[neurons_count];
                    _BestVariantW[i] = (double[,])Network._Layers[i].Clone();
                    _BestVariantOffsetW[i] = (double[])Network._OffsetsWeights[i].Clone();
                }
            }

            public override double Teach(double[] Input, double[] Output, double[] Expected)
            {
                throw new NotImplementedException();
            }
            
            /// <summary>Установить значение оптимальной архитектуры сети</summary>
            public override void SetBestVariant() => throw new NotImplementedException();
        }
    }
}
