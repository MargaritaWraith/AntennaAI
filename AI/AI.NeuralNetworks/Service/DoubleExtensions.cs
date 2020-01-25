using static System.Math;

namespace System
{
    /// <summary>Класс методов-расширений для вещественных чисел</summary>
    internal static class DoubleExtensions
    {
        /// <summary>Адаптивное округление</summary>
        /// <param name="x">Округляемая величина</param>
        /// <param name="n">Количество значащих разрядов</param>
        /// <returns>Число с указанным количеством значащих разрядов</returns>
        public static double RoundAdaptive(this double x, int n = 1)
        {
            if (x.Equals(0.0))
                return 0.0;
            if (double.IsNaN(x) || double.IsInfinity(x))
                return x;
            var sign = Sign(x);
            x = x.GetAbs();
            var pow = Pow(10.0, (double)((int)Log10(x) - 1));
            return Round(x / pow, n - 1) * pow * (double)sign;
        }

        /// <summary>Модуль числа</summary>
        /// <param name="x">Действительное вещественное число</param>
        /// <returns>Модуль числа</returns>
        public static double GetAbs(this double x) => !double.IsNaN(x) ? Abs(x) : double.NaN;
    }
}
