using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using AntennaAI.RT.Digital.Signals;
using static System.Math;

namespace AntennaAI.RT.Digital.Spectra
{
    /// <summary>Спектр цифрового сигнала</summary>
    public class DigitalSpectrum : IEnumerable<DigitalSpectrum.Sample>
    {
        public readonly struct Sample
        {
            public double Frequency { get; }
            public Complex Value { get; }

            public double Re => Value.Real;
            public double Im => Value.Imaginary;
            public double Abs => Value.Magnitude;
            public double Arg => Value.Phase;

            public Sample(double Frequency, Complex Value)
            {
                this.Frequency = Frequency;
                this.Value = Value;
            }

            public override string ToString() => $"[{Frequency}]:({Re}, {Im})[{Abs} *e^{Arg}]";

            public static implicit operator Complex(Sample v) => v.Value;
            public static implicit operator double(Sample v) => v.Abs;
        }

        /// <summary>Спектральные составляющие</summary>
        private readonly Complex[] _Samples;

        /// <summary>Смещение сигнала во времени</summary>
        private readonly double _n0;

        /// <summary>Разрешающая способность спектра</summary>
        public double df { get; }

        /// <summary>Число спектральных составляющих</summary>
        public int SamplesCount => _Samples.Length;

        /// <summary>Максимальное значение частоты в спектре</summary>
        public double MaximumFrequency => df * SamplesCount;

        /// <summary>Доступ (только для чтения) к спектральным составляющим спектра</summary>
        /// <param name="m">Номер спектральной составляющей спектра</param>
        /// <returns>Комплексное значение спектральной составляющей с указанным именем</returns>
        public ref readonly Complex this[int m] => ref _Samples[m];

        /// <summary>Инициализация нового экземпляра <see cref="DigitalSpectrum"/></summary>
        /// <param name="df">Разрешающая способность спектра</param>
        /// <param name="Samples">Спектральные составляющие</param>
        /// <param name="n0">Смещение сигнала во времени</param>
        public DigitalSpectrum(double df, Complex[] Samples, double n0 = 0)
        {
            this.df = df;
            _Samples = Samples;
            _n0 = n0;
        }

        /// <summary>Вычисление цифрового сигнала на основе цифрового спектра</summary>
        public DigitalSignal GetSignal()
        {
            var S = _Samples;
            var M = S.Length;
            var signal = new double[M];

            var w0 = 2 * PI / M;
            for (var n = 0; n < M; n++)
            {
                double signal_sample = 0;
                var arg = w0 * n;
                for (var m = 0; m < M; m++)
                    signal_sample += S[m].Real * Cos(arg * m) - S[m].Imaginary * Sin(arg * m);
                signal[n] = signal_sample;
            }

            var dt = 1 / MaximumFrequency;
            return new DigitalSignal(dt: dt, signal, _n0 * dt);
        }

        public IEnumerator<Sample> GetEnumerator()
        {
            var samples = _Samples;
            var samples_count = samples.Length;
            var df = this.df;
            for (var m = 0; m < samples_count; m++) 
                yield return new Sample(m * df, samples[m]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
