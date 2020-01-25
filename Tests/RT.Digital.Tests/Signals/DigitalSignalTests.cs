using System;
using AntennaAI.RT.Digital.Signals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RT.Digital.Tests.Signals
{
    [TestClass]
    public class DigitalSignalTests
    {
        private const double A0 = 1;
        private const double f0 = 10;
        private const double fd = 10 * f0;
        private const double dt = 1 / fd;
        private const double T0 = 1 / f0;
        private const int N = (int)(T0 / dt);
        private const double pi2 = Math.PI * 2;

        private static double SignalFunction(double t) => A0 * Math.Sin(pi2 * f0 * t);

        [TestMethod]
        public void SignalPower_of_1Sin_Equal_05()
        {
            var signal_samples = new double[N];
            for (var n = 0; n < N; n++)
                signal_samples[n] = SignalFunction(n * dt);

            var signal = new DigitalSignal(dt, signal_samples);

            var power = signal.Power;

            Assert.That.Value(power).IsEqual(0.5, 1.12e-6);
        }
    }
}
