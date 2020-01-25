namespace System.Collections.Generic
{
    /// <summary>Класс методов-расширений для интерфейса <see cref="IEnumerable{T}"/></summary>
    internal static class EnumerableExtensions
    {
        /// <summary>Выполнение действия для всех элементов коллекции с указанием индекса элемента</summary>
        /// <param name="collection">Коллекция элементов</param>
        /// <param name="Action">Действие над элементом</param>
        /// <param name="index">Смещение индекса элемента коллекции</param>
        /// <typeparam name="T">Тип элемента коллекции</typeparam>
        public static void Foreach<T>(this IEnumerable<T> collection, Action<T, int> Action, int index = 0)
        {
            switch (collection)
            {
                case T[] array:
                {
                    var i = 0;
                    for (var length = array.Length; i < length; ++i)
                        Action(array[i], index++);
                    break;
                }
                case IList<T> list:
                {
                    var i = 0;
                    for (var count = list.Count; i < count; ++i)
                        Action(list[i], index++);
                    break;
                }
                case IList list:
                {
                    var i = 0;
                    for (var count = list.Count; i < count; ++i)
                        Action((T)list[i], index++);
                    break;
                }
                default:
                    using (var enumerator = collection.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            var current = enumerator.Current;
                            Action(current, index++);
                        }
                        break;
                    }
            }
        }
    }
}
