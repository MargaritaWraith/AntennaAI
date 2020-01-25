using System.Numerics;
using static System.Math;

namespace AntennaAI.RT.Antennas
{
    /// <summary>Симметричный вибратор, расположенный вдоль оси OX</summary>
    public class Vibrator : DirectionDependent
    {
        private const double __Pi2 = 2 * PI;

        /// <summary>Длина вибратора, нормированная к длине волны</summary>
        public double Length { get; }

        /// <summary>Инициализация нового <see cref="Vibrator"/></summary>
        /// <param name="Length">Длина вибратора, нормированная к длине волны</param>
        public Vibrator(double Length) => this.Length = Length;

        public override Complex Pattern(double Theta, double Phi)
        {
            var length = __Pi2 * Length;
            return (Cos(length * Sin(Theta)) - Cos(length)) / Cos(Theta);
        }
    }
}
