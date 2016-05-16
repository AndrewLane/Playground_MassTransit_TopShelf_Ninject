using System;

namespace Services.Math
{
    /// <summary>
    /// Component for generating random colors
    /// </summary>
    public interface IGenerateRandomColors
    {
        ConsoleColor GetRandomColor();
    }
}
