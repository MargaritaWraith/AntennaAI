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
    }
}
