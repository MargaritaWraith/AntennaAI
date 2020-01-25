namespace System
{
    /// <summary>Класс методов-расширений для массивов</summary>
    internal static class ArrayExtensions
    {
        public static void Deconstruct<T>(this T[,] array, out int Rows, out int Cols)
        {
            Rows = array.GetLength(0);
            Cols = array.GetLength(1);
        }
    }
}
