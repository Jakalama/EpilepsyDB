using EpilepsieDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Models
{
    public class BlockTest : AbstractTest
    {
        public BlockTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(123)]
        [InlineData(321)]
        public void ScanID_ReturnsCorrect(int expected)
        {
            // set
            var block = new Block();
            block.ScanID = expected;

            // act
            var result = block.ScanID;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Scan_ReturnsCorrect()
        {
            // set
            var expected = new Scan();
            var block = new Block();
            block.Scan = expected;

            // act
            var result = block.Scan;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("12.05.2007")]
        [InlineData("05.12.2007")]
        public void Starttime_ReturnsCorrect(string date)
        {
            // set
            var expected = DateTime.Parse(date);
            var block = new Block();
            block.Starttime = expected;

            // act
            var result = block.Starttime;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("12.05.2007")]
        [InlineData("05.12.2007")]
        public void Endtime_ReturnsCorrect(string date)
        {
            // set
            var expected = DateTime.Parse(date);
            var block = new Block();
            block.Endtime = expected;

            // act
            var result = block.Endtime;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0.123)]
        [InlineData(321.0)]
        public void GapToPrevious_ReturnsCorrect(float expected)
        {
            // set
            var block = new Block();
            block.GapToPrevious = expected;

            // act
            var result = block.GapToPrevious;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ScanID_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("ScanID", typeof(Block)));
        }

        [Fact]
        public void Starttime_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Starttime", typeof(Block)));
        }

        [Fact]
        public void Endtime_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Endtime", typeof(Block)));
        }

        [Fact]
        public void GapToPrevious_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("GapToPrevious", typeof(Block)));
        }
    }
}
