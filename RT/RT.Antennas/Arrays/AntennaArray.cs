using System;
using System.Linq;
using System.Numerics;
using AntennaAI.RT.Base.Signals;

namespace AntennaAI.RT.Antennas.Arrays
{
    /// <summary>Антенная решётка</summary>
    public class AntennaArray : DirectionDependent
    {
        /// <summary>Элементы антенной решётки</summary>
        private readonly AntennaArrayItem[] _Items;

        /// <summary>Инициализация нового экземпляра <see cref="AntennaArray"/></summary>
        /// <param name="Items">Массив элементов антенной решётки</param>
        public AntennaArray(params AntennaArrayItem[] Items) => _Items = Items ?? throw new ArgumentNullException(nameof(Items));

        /// <summary>Диаграмма направленности представляет собой сумму диаграмм направленности антенных элементов</summary>
        public override Complex Pattern(double Theta, double Phi)
        {
            Complex sum = default;
            foreach (var item in _Items)
                sum += item.Pattern(Theta, Phi);
            return sum;
        }

        /// <summary>Сигнал на выходе антенной решётки представляет собой сумму сигналов с выходов антенных элементов</summary>
        public override Signal GetSignal(Signal s, double Theta, double Phi)
        {
            var signals = _Items.Select(item => item.GetSignal(s, Theta, Phi)).ToArray();
            var result_signal = new Signal(t =>
            {
                double sum = 0;
                for (var i = 0; i < signals.Length; i++)
                    sum += signals[i](t);
                return sum;
            });
            return result_signal;
        }
    }
}
