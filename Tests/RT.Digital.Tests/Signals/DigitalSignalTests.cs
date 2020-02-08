using System.Diagnostics;
using System.Linq;
using AntennaAI.RT.Digital.Signals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Math;

namespace RT.Digital.Tests.Signals
{
    [TestClass]
    public class DigitalSignalTests
    {
        private const double eps = 1e-13;

        private const double A0 = 2;
        private const double A1 = 10;
        private const double f0 = 10;
        private const double Kfd = 8;
        private const double fd = Kfd * f0;
        private const double dt = 1 / fd;
        private const int N = 16;
        private const double pi2 = PI * 2;

        private static double SignalFunction(double t) => A1 * Sin(pi2 * f0 * t) + A0;

        [TestMethod]
        public void SignalPower_of_1Sin_Equal_05()
        {
            var signal_samples = new double[N];
            for (var n = 0; n < N; n++)
                signal_samples[n] = SignalFunction(n * dt);

            var signal = new DigitalSignal(dt, signal_samples);

            var power = signal.Power;

            Assert.That.Value(power).IsEqual(A1 * A1 / 2 + A0 * A0, eps);
        }

        [TestMethod]
        public void Sin_Spectrum()
        {
            var signal_smaples = new double[N];
            for (var n = 0; n < N; n++)
            {
                var v = SignalFunction(n * dt);
                signal_smaples[n] = Abs(v) < 1e-10 ? 0 : v;
            }

            var signal = new DigitalSignal(dt, signal_smaples);

            var spectrum = signal.GetSpectrum();

            const double to_deg = 180 / PI;
            Assert.That.Value(spectrum)
                .Where(S => S.SamplesCount).Check(Count => Count.IsEqual(N))
                .Where(S => S[0].Magnitude).Check(S0 => S0.IsEqual(A0, eps))
                .Where(S => S[1].Magnitude).Check(S1 => S1.IsEqual(0, eps))
                .Where(S => S[2]).Check(S2 => S2
                    .Where(v => v.Magnitude).Check(v => v.IsEqual(A1 / 2, eps))
                    .Where(v => v.Phase * to_deg).Check(v => v.IsEqual(-90, eps)))
                .Where(S => S[2].Magnitude).Check(S1 => S1.IsEqual(A1 / 2, eps))
                .Where(S => S[3].Magnitude).Check(S1 => S1.IsEqual(0, eps))
                .Where(S => S[14].Magnitude).Check(S1 => S1.IsEqual(A1 / 2, eps))
                .Where(S => S[14]).Check(S14 => S14
                    .Where(v => v.Magnitude).Check(v => v.IsEqual(A1 / 2, eps))
                    .Where(v => v.Phase * to_deg).Check(v => v.IsEqual(90, eps)))
                .Where(S => S[15].Magnitude).Check(S1 => S1.IsEqual(0, eps));

        }
    }
}
