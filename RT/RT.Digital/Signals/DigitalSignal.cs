using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AntennaAI.RT.Digital.Spectra;
using static System.Math;

namespace AntennaAI.RT.Digital.Signals
{
    /// <summary>Цифровой сигнал</summary>
    public class DigitalSignal : IEnumerable<DigitalSignal.Sample>
    {
        public readonly struct Sample
        {
            public double Time { get; }
            public double Value { get; }

            public Sample(double Time, double Value)
            {
                this.Time = Time;
                this.Value = Value;
            }

            public override string ToString() => $"[{Time}]:{Value}";
        }

        /// <summary>Массив выборки отсчётов цифрового сигнала</summary>
        private readonly double[] _Samples;

        /// <summary>Начальное смещение сигнала во времени</summary>
        public double t0 { get; }

        /// <summary>Период времени дискретизации</summary>
        public double dt { get; }

        /// <summary>Число отсчётов выборки сигнала</summary>
        public int SamplesCount => _Samples.Length;

        /// <summary>Длина сигнала во времени</summary>
        public double SignalTime => SamplesCount * dt;

        /// <summary>Сумма всех отсчётов сигнала</summary>
        public double SamplesSum => _Samples.Sum();

        /// <summary>Сумма квадратов всех отсчётов сигнала</summary>
        public double SamplesSum2 => _Samples.Sum(s => s * s);

        /// <summary>Энергия сигнала</summary>
        public double Energy => SamplesSum2 * dt;

        /// <summary>Мощность сигнала</summary>
        public double Power => SamplesSum2 / SamplesCount;

        /// <summary>Среднее значение выборки сигнала</summary>
        public double Average => _Samples.Average();

        /// <summary>Дисперсия выборки сигнала</summary>
        public double StdDev
        {
            get
            {
                var sum = 0d;
                var sum2 = 0d;
                var count = _Samples.Length;
                for (var i = 0; i < count; i++)
                {
                    var s = _Samples[i];
                    sum += s;
                    sum2 += s * s;
                }

                return (sum2 / count - sum) / count;
            }
        }

        /// <summary>Доступ по ссылке (только для чтения) к значениям выборки сигнала</summary>
        /// <param name="n">Номер отсчёта сигнала</param>
        /// <returns>Значение выборки сигнала с указанным номером</returns>
        public ref readonly double this[int n] => ref _Samples[n];

        /// <summary>Инициализация нового экземпляра <see cref="DigitalSignal"/></summary>
        /// <param name="dt">Период времени дискретизации</param>
        /// <param name="Samples">Массив значений выборки сигнала</param>
        /// <param name="t0">Смещение сигнала во времени</param>
        public DigitalSignal(double dt, double[] Samples, double t0 = 0)
        {
            this.dt = dt;
            _Samples = Samples;
            this.t0 = t0;
        }

        /// <summary>Вычисление спектра сигнала</summary>
        public DigitalSpectrum GetSpectrum()
        {
            var s = _Samples;
            var count = s.Length;
            var spectrum = new Complex[count];

            var w0 = -2 * PI / count;
            for (var m = 0; m < count; m++)
            {
                var sample = Complex.Zero;
                var arg = w0 * m;
                for(var n = 0; n < count; n++) 
                    sample += new Complex(s[n] * Cos(arg * n), s[n] * Sin(arg * n));

                spectrum[m] = sample / count;
            }
            return new DigitalSpectrum(df: 1 / SignalTime, spectrum, n0: t0 / dt);
        }

        /// <inheritdoc />
        public IEnumerator<Sample> GetEnumerator()
        {
            var samples = _Samples;
            var samples_count = samples.Length;
            var time_step = dt;
            for(var n = 0; n < samples_count; n++)
                yield return new Sample(n * time_step, samples[n]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
