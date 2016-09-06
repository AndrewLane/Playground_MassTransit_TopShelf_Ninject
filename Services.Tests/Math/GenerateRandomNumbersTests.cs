using FluentAssertions;
using NUnit.Framework;
using Services.Math.Impl;
using System;
using System.Diagnostics;

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

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(5000)]
        public void TestGetRandomIntWorksWithNoRange(int min)
        {
            var objectUnderTest = new GenerateRandomNumbers();
            objectUnderTest.GetRandomInt(inclusiveMin: min, inclusiveMax: min).Should().Be(min);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(50)]
        [TestCase(500)]
        [TestCase(5000)]
        public void TestGetRandomIntGetsAllNumbersInRangeEventually(int max)
        {
            var maxTimeToRun = TimeSpan.FromSeconds(1); // don't let the test churn for more than a second
            var stopWatch = new Stopwatch();            

            var objectUnderTest = new GenerateRandomNumbers();

            bool[] found = new bool[max+1];

            bool done;

            stopWatch.Start();
            do
            {
                var randomInt = objectUnderTest.GetRandomInt(inclusiveMin: 0, inclusiveMax: max);
                found[randomInt] = true;

                //assume we're done until we're proven otherwise
                done = true;
                for (int i = 0; i <= max; i++)
                {
                    if (found[i]) continue;
                    done = false;
                    break;
                }

                if (stopWatch.Elapsed > maxTimeToRun)
                {
                    throw new Exception("Test took too long to succeed");
                }
            } while (done == false);

            //if we get here, we've succeeded
            stopWatch.Stop();
        }
    }
}
