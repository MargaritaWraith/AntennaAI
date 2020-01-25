using System;
using AntennaAI.AI.NeuralNetworks.Interfaces;

namespace AntennaAI.AI.NeuralNetworks
{
    /// <summary>Класс методов-расширений для нейронной сети</summary>
    public static class NeuralNetworkExtensions
    {
        /// <summary>Рассчитать отклик сети для входного воздействия</summary>
        /// <param name="Network">Нейронная сеть, осуществляющая обработку данных</param>
        /// <param name="Input">Вектор входного воздействия для сети</param>
        /// <returns>Вновь созданный вектор отклика сети</returns>
        public static double[] Process(this INeuralNetwork Network, double[] Input)
        {
            if (Network is null) throw new ArgumentNullException(nameof(Network));
            if (Input is null) throw new ArgumentNullException(nameof(Input));

            var result = new double[Network.OutputsCount];
            Network.Process(Input, result);
            return result;
        }

        /// <summary>Метод расчёта отклика сети с вычислением ошибки по ожидаемому результату</summary>
        /// <param name="Network">Нейронная сеть, осуществляющая обработку данных</param>
        /// <param name="Input">Вектор входного воздействия для сети</param>
        /// <param name="Output">Вектор отклика сети</param>
        /// <param name="ExpectedOutput">Вектор ожидаемого результата на выходе сети</param>
        /// <param name="Error">Вектор ошибки, рассчитываемый как половина квадрата разности ячеек ожидаемого и полученного векторов отклика сети</param>
        public static void Process(
            this INeuralNetwork Network,
            double[] Input,
            double[] Output,
            double[] ExpectedOutput,
            double[] Error)
        {
            Network.Process(Input, Output);

            for (var i = 0; i < Output.Length; i++)
            {
                var delta = ExpectedOutput[i] - Output[i];
                Error[i] = 0.5 * delta * delta;
            }
        }
    }
}
