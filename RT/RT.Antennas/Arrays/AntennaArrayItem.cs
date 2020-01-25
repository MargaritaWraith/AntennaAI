using System.Numerics;
using AntennaAI.Mathematics.Geometry;
using AntennaAI.RT.Base.Signals;
using static System.Math;

namespace AntennaAI.RT.Antennas.Arrays
{
    /// <summary>Элемент антенной решётки</summary>
    public class AntennaArrayItem : DirectionDependent
    {
        /// <summary>Антенна, выступающая в качестве антенного элемента решётки</summary>
        private readonly Antenna _Antenna;

        /// <summary>Вектор размещения антенного элемента в апертуре</summary>
        private readonly Vector3D _Vector;

        /// <summary>Вектор размещения антенного элемента в апертуре</summary>
        public Vector3D Vector => _Vector;

        /// <summary>Инициализация нового экземпляра <see cref="AntennaArrayItem"/></summary>
        /// <param name="Antenna">Антенна, выступающая в качестве антенного элемента решётки</param>
        /// <param name="Vector">Вектор размещения антенного элемента в апертуре</param>
        public AntennaArrayItem(Antenna Antenna, Vector3D Vector)
        {
            _Antenna = Antenna;
            _Vector = Vector;
        }

        /// <summary>
        /// Разность хода волн между точкой размещения антенного элемента и началом координат
        /// (нормированное к длине волны)
        /// </summary>
        /// <param name="Theta">Угол места падения волны</param>
        /// <param name="Phi">Угол азимута падения волны</param>
        /// <returns>
        /// Разность хода волн между точкой размещения антенного элемента и фазовым центром,
        /// нормированная к длине волны
        /// </returns>
        private double GetSpaceOffset(double Theta, double Phi) => _Vector.GetProjectionTo(Theta, Phi);

        /// <summary>
        /// Диаграмма направленности антенного элемента приобретет фазовое смещение,
        /// соответствующее смещению его из фазового центра системы
        /// </summary>
        public override Complex Pattern(double Theta, double Phi)
        {
            var f = _Antenna.Pattern(Theta, Phi);
            var arg = 2 * PI * GetSpaceOffset(Theta, Phi);
            return f * new Complex(Cos(arg), Sin(arg));
        }

        /// <summary>Сигнал на выходе антенного элемента задержан во времени на разность хода волн</summary>
        public override Signal GetSignal(Signal s, double Theta, double Phi)
        {
            //const double speed_of_light = 3e8;
            var time_offset = GetSpaceOffset(Theta, Phi);// / speed_of_light; // Деление на скорость света отключено потому, что время сигнала нормировано к частоте
            return base.GetSignal(t => s(t - time_offset), Theta, Phi);
        }
    }
}