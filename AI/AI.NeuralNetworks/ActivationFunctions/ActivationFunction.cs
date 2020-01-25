using System;
using System.Collections.Generic;
using System.Text;

namespace AntennaAI.AI.NeuralNetworks.ActivationFunctions
{
    /// <summary>Функция активации</summary>
    public abstract class ActivationFunction
    {
        /// <summary>Значение функции активации</summary>
        /// <param name="x">Взвешенная сумма входов нейронов</param>
        /// <returns>Значение выхода нейрона</returns>
        public abstract double Value(double x);

        /// <summary>Производная функции активации</summary>
        /// <param name="x">Значение на выходе нейрона</param>
        /// <returns>Значение производной функции активации</returns>
        public abstract double DiffValue(double x);
    }
}
