namespace System
{
    /// <summary>Класс методов-расширений для массивов</summary>
    internal static class ArrayExtensions
    {
        /// <summary>Создать копию массива с перемешанным содержимым</summary>
        /// <param name="array">Исходный массив</param>
        /// <typeparam name="T">Тип элементов массива</typeparam>
        /// <param name="rnd">Генератор случайных чисел</param>
        /// <returns>Копия исходного массива с перемешанным содержимым</returns>
        public static T[] Mix<T>(this T[] array, Random rnd) => ((T[])array.Clone()).MixRef<T>(rnd);

        /// <summary>Перемешать массив</summary>
        /// <typeparam name="T">Тип элементов массива</typeparam>
        /// <param name="array">Перемешиваемый массив</param>
        /// <param name="rnd">Генератор случайных чисел</param>
        /// <returns>Исходный массив с перемешанным содержимым</returns>
        public static T[] MixRef<T>(this T[] array, Random rnd)
        {
            var length = array.Length;
            if (rnd is null)
                rnd = new Random();
            var obj = array[0];
            var i = 0;
            for (var j = 1; j <= length; ++j)
                array[i] = array[i = rnd.Next(length)];
            array[i] = obj;
            return array;
        }

        /// <summary>Определить индекс максимального элемента в массиве</summary>
        /// <param name="array">Исследуемый массив</param>
        /// <returns>Индекс максимального элемента</returns>
        public static int GetMaxIndex(this double[] array)
        {
            var max = double.NegativeInfinity;
            var max_index = -1;
            for (var i = 0; i < array.Length; ++i)
            {
                if (array[i] <= max) continue;
                max = array[i];
                max_index = i;
            }
            return max_index;
        }
    }
}
