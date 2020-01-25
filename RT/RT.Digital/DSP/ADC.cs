using System;
using System.Collections.Generic;
using System.Text;
using AntennaAI.RT.Base.Signals;
using AntennaAI.RT.Digital.Signals;
using static System.Math;

namespace AntennaAI.RT.Digital.DSP
{
    /// <summary>Аналого-цифровой преобразователь</summary>
    public class ADC
    {
        /// <summary>Частота дискретизации</summary>
        private readonly double _fd;

        /// <summary>Число быт кода</summary>
        private readonly int _BitCount;

        /// <summary>Минимальное допустимое значение входного ограничителя</summary>
        private readonly double _MinValue;

        /// <summary>Максимальное допустимое значение входного ограничителя</summary>
        private readonly double _MaxValue;

        /// <summary>Частота дискретизации</summary>
        public double fd => _fd;

        /// <summary>Число быт кода</summary>
        public int BitCount => _BitCount;

        /// <summary>Число интервалов дискретизации</summary>
        public int IntervalsCount => 1 >> _BitCount;

        /// <summary>Минимальное допустимое значение входного ограничителя</summary>
        public double MinValue => _MinValue;

        /// <summary>Максимальное допустимое значение входного ограничителя</summary>
        public double MaxValue => _MaxValue;

        /// <summary>Максимальная допустимая амплитуда сигнала</summary>
        public double Diapason => _MaxValue - _MinValue;

        /// <summary>Разрешающая способность по амплитуде (шаг сетки квантования)</summary>
        public double Resolution => Diapason / IntervalsCount;

        /// <summary>Инициализация нового экземпляра <see cref="ADC"/></summary>
        /// <param name="fd">Частота дискретизации</param>
        /// <param name="BitCount">Число бит</param>
        /// <param name="MinValue">Минимальное допустимое значение входного ограничителя</param>
        /// <param name="MaxValue">Максимальное допустимое значение входного ограничителя</param>
        public ADC(double fd, int BitCount, double MinValue = 0, double MaxValue = 5)
        {
            if(fd <= 0) throw new ArgumentOutOfRangeException(nameof(fd), fd, "Частота должна быть величиной больше 0");
            if(BitCount < 1) throw new ArgumentOutOfRangeException(nameof(BitCount), BitCount, "Число бит должно быть больше 0");
            if(Abs(MaxValue - MinValue) < double.Epsilon)
                throw new InvalidOperationException("Интервалы минимального и максимального допустимого значения совпадают");

            _fd = fd;
            _BitCount = BitCount;
            _MinValue = Min(MinValue, MaxValue);
            _MaxValue = Max(MinValue, MaxValue);
        }

        /// <summary>Квантование амплитуды сигнала по уровню</summary>
        /// <param name="Value">Квантуемое значение амплитуды сигнала</param>
        /// <param name="Resolution">Разрешающая способность сетки квантования</param>
        /// <returns>Квантованное (округлённое до ближайшего значения сетки квантования) значение амплитуды</returns>
        private static double Quantization(double Value, double Resolution) => Round(Value / Resolution) * Resolution;

        /// <summary>Ограничение амплитуды</summary>
        /// <param name="Value">Ограничиваемое значение амплитуды</param>
        /// <param name="MinValue">Нижняя граница амплитуды</param>
        /// <param name="MaxValue">Верхняя граница амплитуды</param>
        /// <returns>Значение амплитуды в пределах заданного интервала</returns>
        private static double Limit(double Value, double MinValue, double MaxValue) => Max(MinValue, Min(MaxValue, Value));

        /// <summary>Дискретизация аналогового сигнала</summary>
        /// <param name="s">Дискретизируемый аналоговый сигнал</param>
        /// <param name="T">Период времени формирования выборки</param>
        /// <returns>Цифровой сигнал</returns>
        public DigitalSignal Discretize(Signal s, double T)
        {
            var N = (int)(T * _fd);
            var samples = new double[N];
            var dt = 1 / _fd;
            var min = _MinValue;
            var max = _MaxValue;
            var resolution = Resolution;

            for (var n = 0; n < N; n++)
                samples[n] = Quantization(Limit(s(n * dt), min, max), resolution);
            return new DigitalSignal(dt, samples);
        }
    }
}
