using static System.Math;

namespace System
{
    /// <summary>Класс методов-расширений для <see cref="Random"/></summary>
    public static class RandomExtensions
    {
        /// <summary>Случайное число с нормальным распределением</summary>
        /// <param name="rnd">Датчик случайных чисел</param>
        /// <param name="sigma">Среднеквадратичное отклонение</param>
        /// <param name="mu">Математическое ожидание</param>
        /// <returns>Случайное число с нормальным распределением</returns>
        public static double NextNormal(this Random rnd, double sigma = 1.0, double mu = 0.0) => 
            mu + sigma * (Sqrt(-2 * Log(1 - rnd.NextDouble())) * Sin(2 * PI * (1 - rnd.NextDouble())));
    }
}
