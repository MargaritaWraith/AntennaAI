using System;
using AntennaAI.AI.NeuralNetworks.Interfaces;

namespace AntennaAI.AI.NeuralNetworks
{
    /// <summary>Нейронный процессор</summary>
    /// <typeparam name="TInput">Тип входных данных</typeparam>
    /// <typeparam name="TOutput">Тип выходных данных</typeparam>
    public class NeuralProcessor<TInput, TOutput>
    {
        /// <summary>Метод преобразования типа входных данных в массив вещественных чисел - массив признаков</summary>
        /// <param name="InputValue">Входной объект</param>
        /// <param name="NetworkInput">Массив входных признаков, подаваемый на вход нейронной сети</param>
        /// <remarks>Цель метода определить - как входной объект отображается (проецируется) на массив входов сети</remarks>
        public delegate void InputFormatter(TInput InputValue, double[] NetworkInput);

        /// <summary>Метод преобразования массива вещественных чисел - массива выходных признаков нейронной сети в объект выхода</summary>
        /// <param name="NetworkOutput">Массив выходных признаков нейронной сети</param>
        /// <returns>Объект, сформированный на основе массива признаков, рассчитанных нейронной сетью</returns>
        public delegate TOutput OutputFormatter(double[] NetworkOutput);

        /// <summary>Метод преобразования выходного значения в массив значений выхода сети (используется в процессе обучения)</summary>
        /// <param name="Output">Выходное значение</param>
        /// <param name="NetworkOutput">Массив значений на выходе нейронной сети</param>
        public delegate void BackOutputFormatter(TOutput Output, double[] NetworkOutput);

        /// <summary>Нейронная сеть, осуществляющая преобразование входного набора признаков в выходной</summary>
        private readonly INeuralNetwork _Network;

        /// <summary>Метод формирования входного набора признаков на основе входного объекта</summary>
        private readonly InputFormatter _InputFormatter;

        /// <summary>Метод формирования выходного объекта на основе набора признаков, сформированного нейронной сетью</summary>
        private readonly OutputFormatter _OutputFormatter;

        /// <summary>Массив вещественных чисел - вектор входных признаков</summary>
        private readonly double[] _Input;

        /// <summary>Массив вещественных чисел - вектор выходных признаков</summary>
        private readonly double[] _Output;

        /// <summary>Очищать массив входа сети перед обработкой данных</summary>
        private bool _ClearInput = true;

        /// <summary>Очищать массив входа сети перед обработкой данных</summary>
        public bool ClearInput { get => _ClearInput; set => _ClearInput = value; }

        /// <summary>Создать новый нейронный процессор</summary>
        /// <param name="Network">Нейронная сеть</param>
        /// <param name="InputFormatter">Метод формирования вектора признаков входного воздействия</param>
        /// <param name="OutputFormatter">Метод формирования выходного значения на основе вектора признаков, формируемого сетью</param>
        public NeuralProcessor(
            INeuralNetwork Network,
            InputFormatter InputFormatter,
            OutputFormatter OutputFormatter)
        {
            _Network = Network ?? throw new ArgumentNullException(nameof(Network));
            _InputFormatter = InputFormatter ?? throw new ArgumentNullException(nameof(InputFormatter));
            _OutputFormatter = OutputFormatter ?? throw new ArgumentNullException(nameof(OutputFormatter));
            _Input = new double[_Network.InputsCount];
            _Output = new double[_Network.OutputsCount];
        }

        /// <summary>Обработать значение</summary>
        /// <param name="Input">Входное значение</param>
        /// <returns>Выходное значение</returns>
        public TOutput Process(TInput Input)
        {
            if (_ClearInput)
                Array.Clear(_Input, 0, _Input.Length);
            _InputFormatter(Input, _Input);
            _Network.Process(_Input, _Output);
            return _OutputFormatter(_Output);
        }

        /// <summary>Создать учителя сети</summary>
        /// <param name="Configurator">Метод конфигурации учителя</param>
        /// <returns>Учитель нейронной сети</returns>
        public TNetworkTeacher CreateTeacher<TNetworkTeacher>(Action<TNetworkTeacher> Configurator)
            where TNetworkTeacher : class, INetworkTeacher
        {
            if (!(_Network is ITeachableNeuralNetwork teachable_network))
                throw new InvalidOperationException("Сеть не является обучаемой");
            return teachable_network.CreateTeacher(Configurator);
        }

        /// <summary>Создать учителя сети</summary><returns>Учитель нейронной сети</returns>
        public INetworkTeacher CreateTeacher()
        {
            if (!(_Network is ITeachableNeuralNetwork teachable_network))
                throw new InvalidOperationException("Сеть не является обучаемой");
            return teachable_network.CreateTeacher();
        }
    }
}
