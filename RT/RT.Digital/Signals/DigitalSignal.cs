using System.Linq;
using System.Numerics;
using AntennaAI.RT.Digital.Spectra;
using static System.Math;

namespace AntennaAI.RT.Digital.Signals
{
    /// <summary>Цифровой сигнал</summary>
    public class DigitalSignal
    {
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
                var average = Average;
                average *= average;
                return _Samples.Average(s => s * s - average);
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
            var N = s.Length;
            var spectrum = new Complex[N];

            var w0 = -2 * PI / N;
            for (var m = 0; m < N; m++)
            {
                Complex spectrum_sample;
                var arg = w0 * m;
                for(var n = 0; n < N; n++)
                    spectrum_sample += new Complex(s[n] * Cos(arg * n), s[n] * Sin(arg * n));
                spectrum[m] = spectrum_sample / N;
            }
            return new DigitalSpectrum(df: 1 / SignalTime, spectrum, n0: t0 / dt);
        }
    }
}
