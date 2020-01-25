﻿using System.Numerics;
using AntennaAI.RT.Digital.Signals;
using static System.Math;

namespace AntennaAI.RT.Digital.Spectra
{
    /// <summary>Спектр цифрового сигнала</summary>
    public class DigitalSpectrum
    {
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
    }
}