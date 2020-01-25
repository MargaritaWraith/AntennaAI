using AntennaAI.RT.Base.Signals;

namespace AntennaAI.RT.Antennas
{
    /// <summary>Антенна с направленными свойствами</summary>
    public abstract class DirectionDependent : Antenna
    {
        /// <summary>Амплитуда сигнала на выходе антенны пропорциональна амплитуде диаграммы направленности</summary>
        public override Signal GetSignal(Signal s, double Theta, double Phi) 
            => t => s(t) * Pattern(Theta, Phi).Magnitude;
    }
}