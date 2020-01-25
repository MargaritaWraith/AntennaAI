﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AntennaAI.AI.NeuralNetworks.ActivationFunctions
{
    /// <summary>Функция активации</summary>
    public abstract class ActivationFunction
    {
        /// <summary>Линейная функция</summary>
        public static Linear Linear => new Linear();

        /// <summary>Линейная функция с параметрами</summary>
        /// <param name="K">Производная</param>
        /// <param name="B">Смещение</param>
        public static Linear GetLinear(double K, double B = 0) => new Linear(K, B);

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
