using FluentAssertions;
using NUnit.Framework;
using Services.Math.Impl;
using System;

namespace Services.Tests.Math
{
    /// <summary>
    /// Tests for the GenerateRandomNumbers class
    /// </summary>
    [TestFixture]
    public class GenerateRandomNumbersTests
    {
        [Test]
        public void TestGetRandomIntThrowsExceptionWithBadInput()
        {
            var objectUnderTest = new GenerateRandomNumbers();

            Action actionThatShouldThowException = () => objectUnderTest.GetRandomInt(1, 0);
            actionThatShouldThowException.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}
