using System;
using System.Numerics;

namespace AntennaAI.RT.Antennas
{
    /// <summary>Элемент Гюйгенса</summary>
    public class Huygens : DirectionDependent
    {
        public override Complex Pattern(double Theta, double Phi) => (1 + Math.Cos(Theta)) / 2;
  }
}