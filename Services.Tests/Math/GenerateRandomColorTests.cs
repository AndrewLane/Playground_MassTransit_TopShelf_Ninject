using FluentAssertions;
using Moq;
using NUnit.Framework;
using Services.Math;
using Services.Math.Impl;
using System;

namespace Services.Tests.Math
{
    /// <summary>
    /// Tests for the GenerateRandomColor class
    /// </summary>
    [TestFixture]
    public class GenerateRandomColorTests
    {
        [TestCase(0, ConsoleColor.Red)]
        [TestCase(1, ConsoleColor.Yellow)]
        [TestCase(2, ConsoleColor.Magenta)]
        [TestCase(3, ConsoleColor.Blue)]
        [TestCase(4, ConsoleColor.Cyan)]
        [TestCase(5, ConsoleColor.DarkGreen)]
        [TestCase(6, ConsoleColor.White)]
        [TestCase(7, ConsoleColor.DarkBlue)]
        public void TestGetRandomColor(int randomNumberReturned, ConsoleColor expectedColor)
        {
            var randomNumberMock = new Mock<IGenerateRandomNumbers>(MockBehavior.Strict);
            randomNumberMock.Setup(mock => mock.GetRandomInt(0, 7)).Returns(randomNumberReturned);

            var objectUnderTest = new GenerateRandomColor(randomNumberMock.Object);

            objectUnderTest.GetRandomColor().Should().Be(expectedColor);
        }

        [TestCase(-1)]
        [TestCase(8)]
        [TestCase(100)]
        [TestCase(1000)]
        public void TestGetRandomColorWithBadRandomNumbersGenerated(int badRandomNumber)
        {
            var randomNumberMock = new Mock<IGenerateRandomNumbers>(MockBehavior.Strict);
            randomNumberMock.Setup(mock => mock.GetRandomInt(0, 7)).Returns(badRandomNumber);

            var objectUnderTest = new GenerateRandomColor(randomNumberMock.Object);

            Action actionThatShouldThrowException = () => objectUnderTest.GetRandomColor();
            actionThatShouldThrowException.ShouldThrow<IndexOutOfRangeException>();
        }
    }
}
