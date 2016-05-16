﻿using System;

namespace Services.Math.Impl
{
    internal class GenerateRandomColor : IGenerateRandomColors
    {
        IGenerateRandomNumbers _randomNumberGenerator;

        //a random list of colors to choose from
        static ConsoleColor[] _possibleColors = new[] { ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Magenta,
            ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.DarkGreen, ConsoleColor.White, ConsoleColor.DarkBlue };

        public GenerateRandomColor(IGenerateRandomNumbers randomNumberGenerator)
        {
            if (randomNumberGenerator == null) throw new ArgumentNullException(nameof(randomNumberGenerator));
            _randomNumberGenerator = randomNumberGenerator;
        }

        public ConsoleColor GetRandomColor()
        {
            int index = _randomNumberGenerator.GetRandomInt(inclusiveMin: 0, inclusiveMax: _possibleColors.Length - 1);
            return _possibleColors[index];
        }
    }
}
