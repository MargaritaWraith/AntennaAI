using System.Collections.Generic;

namespace System
{
    /// <summary>Класс методов-расширений для <see cref="Random"/></summary>
    internal static class RandomExtensions
    {
        /// <summary>Последовательность случайных целых чисел в указанном интервале</summary>
        /// <param name="rnd">Датчик случайных чисел</param>
        /// <param name="min">Нижняя граница интервала (входит)</param>
        /// <param name="max">Верхняя граница интервала (не входит)</param>
        /// <param name="count">Размер выборки (если меньше 0), то бесконечная последовательность</param>
        /// <returns>Последовательность случайных целых чисел в указанном интервале</returns>
        public static IEnumerable<int> SequenceInt(this Random rnd, int min, int max, int count = -1)
        {
            if (rnd is null) throw new ArgumentNullException(nameof(rnd));
            if (count == 0) yield break;
            if (count < 0)
                while (true)
                    yield return rnd.Next(min, max);

            for (var i = 0; i < count; ++i)
                yield return rnd.Next(min, max);
        }
    }
}
