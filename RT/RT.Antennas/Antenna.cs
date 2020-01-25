using System.Numerics;
using AntennaAI.RT.Base.Signals;

namespace AntennaAI.RT.Antennas
{
    /// <summary>Антенна</summary>
    public abstract class Antenna
    {
        /// <summary>Комплексная диаграмма направленности</summary>
        /// <param name="Theta">Угол места</param>
        /// <param name="Phi">Угол азимута</param>
        /// <returns>Комплексное значение диаграммы направленности для заданного угла</returns>
        public abstract Complex Pattern(double Theta, double Phi);

        /// <summary>Получить сигнал с выхода антенны для заданного угла падения волны</summary>
        /// <param name="s">Сигнал падающей волны</param>
        /// <param name="Theta">Угол места</param>
        /// <param name="Phi">Угол азимута</param>
        /// <returns>Сигнал на выходе антенны</returns>
        public abstract Signal GetSignal(Signal s, double Theta, double Phi);
    }
}
