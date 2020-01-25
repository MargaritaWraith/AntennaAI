using System.Numerics;
using AntennaAI.RT.Base.Signals;

namespace AntennaAI.RT.Antennas
{
    /// <summary>Всенаправленная антенна</summary>
    public class Uniform : Antenna
    {
        /// <summary>Значение диаграммы направленности для любого значения угла = 1</summary>
        public override Complex Pattern(double Theta, double Phi) => 1;

        /// <summary>Сигнал на выходе антенны соответствует сигналу волны</summary>
        public override Signal GetSignal(Signal s, double Theta, double Phi) => s;
    }
}
