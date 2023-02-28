using EpilepsieDB.Services.Impl;
using EpilepsieDB.Source.Helper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Source
{
    public class BlockTimeIteratorTest : AbstractTest
    {
        public BlockTimeIteratorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(1f, true)]
        [InlineData(1f, false)]
        [InlineData(0f, true)]
        [InlineData(5f, false)]
        public void Constructor_ThrowsNoError(float duration, bool isInterupted)
        {
            // set
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddMinutes(duration);

            // act
            var iterator = new BlockTimeIterator(startTime, endTime, duration, isInterupted);
        }

        [Fact]
        public void Next_ReturnsTimeInfo()
        {
            // set
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddMinutes(1);

            var iterator = new BlockTimeIterator(startTime, endTime, 1, true);

            // act
            var result = iterator.Next(It.IsAny<float>());

            // assert
            Assert.IsType<TimeInfo>(result);
        }

        [Theory]
        [InlineData(1f, true)]
        [InlineData(1f, false)]
        [InlineData(0, true)]
        [InlineData(0, false)]
        [InlineData(10, true)]
        [InlineData(10, false)]
        public void Next_CorrectStartTimeForFirstCall(float duration, bool isInterupted)
        {
            // set
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddMinutes(duration);

            var iterator = new BlockTimeIterator(startTime, endTime, duration, isInterupted);

            // act
            var result = iterator.Next(It.IsAny<float>());

            // assert
            Assert.Equal(startTime, result.StartTime);
        }

        [Theory]
        [InlineData(1f, true)]
        [InlineData(1f, false)]
        [InlineData(0, true)]
        [InlineData(0, false)]
        [InlineData(10, true)]
        [InlineData(10, false)]
        public void Next_CorrectEndTimeForFirstCall(float duration, bool isInterupted)
        {
            // set
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddMinutes(duration);

            var iterator = new BlockTimeIterator(startTime, endTime, duration, isInterupted);

            // act
            var result = iterator.Next(It.IsAny<float>());

            // assert
            Assert.Equal(endTime, result.EndTime);
        }

        [Theory]
        [InlineData(1f)]
        [InlineData(1f)]
        [InlineData(0)]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(10)]
        public void Next_CorrectStartTimeForSecondCall_Interupted(float duration)
        {
            // set
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddMinutes(duration);
            var expected = startTime.AddSeconds(duration);

            var iterator = new BlockTimeIterator(startTime, endTime, duration, true);

            // act
            var result = iterator.Next(duration);
            result = iterator.Next(duration);

            // assert
            Assert.Equal(expected, result.StartTime);
        }

        [Theory]
        [InlineData(1f)]
        [InlineData(1f)]
        [InlineData(0)]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(10)]
        public void Next_CorrectEndTimeForSecondCall_Interupted(float duration)
        {
            // set
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddMinutes(duration);
            var expected = startTime.AddSeconds(2 * duration);

            var iterator = new BlockTimeIterator(startTime, endTime, duration, true);

            // act
            var result = iterator.Next(duration);
            result = iterator.Next(duration);

            // assert
            Assert.Equal(expected, result.EndTime);
        }

        [Theory]
        [InlineData(0f, 1f)]
        [InlineData(0f, 2f)]
        [InlineData(0f, 3f)]
        [InlineData(1f, 2f)]
        [InlineData(2f, 2f)]
        [InlineData(3f, 4f)]
        public void Next_CorrectGapForSecondCall_Interupted(float offset1, float offset2)
        {
            // set
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddMinutes(5);
            var expected = (float)(startTime.AddSeconds(offset2) - endTime).TotalSeconds;

            var iterator = new BlockTimeIterator(startTime, endTime, 1, true);

            // act
            var result1 = iterator.Next(offset1);
            var result2 = iterator.Next(offset2);

            // assert
            Assert.Equal(0, result1.Gap);
            Assert.Equal(expected, result2.Gap);
        }

    }
}
