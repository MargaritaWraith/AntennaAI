namespace AntennaAI.RT.Base.Signals
{
    /// <summary>Аналоговый сигнал, как непрерывная функция времени</summary>
    /// <param name="Time">Значение времени (в секундах)</param>
    /// <returns>Значение амплитуды сигнала в заданный момент времени</returns>
    public delegate double Signal(double Time);
}
