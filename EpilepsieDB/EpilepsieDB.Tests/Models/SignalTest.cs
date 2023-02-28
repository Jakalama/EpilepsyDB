using EpilepsieDB.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Models
{
    public class SignalTest : AbstractTest
    {
        public SignalTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(123)]
        [InlineData(321)]
        public void ScanID_ReturnsCorrect(int expected)
        {
            // set
            var header = new Signal();
            header.ScanID = expected;

            // act
            var result = header.ScanID;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Scan_ReturnsCorrect()
        {
            // set
            var expected = new Scan();
            var header = new Signal();
            header.Scan = expected;

            // act
            var result = header.Scan;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(123)]
        [InlineData(321)]
        public void Channel_ReturnsCorrect(int expected)
        {
            // set
            var header = new Signal();
            header.Channel = expected;

            // act
            var result = header.Channel;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("321")]
        public void Label_ReturnsCorrect(string expected)
        {
            // set
            var header = new Signal();
            header.Label = expected;

            // act
            var result = header.Label;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("321")]
        public void TransducerType_ReturnsCorrect(string expected)
        {
            // set
            var header = new Signal();
            header.TransducerType = expected;

            // act
            var result = header.TransducerType;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("321")]
        public void PhysicalDimension_ReturnsCorrect(string expected)
        {
            // set
            var header = new Signal();
            header.PhysicalDimension = expected;

            // act
            var result = header.PhysicalDimension;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0.123)]
        [InlineData(321.0)]
        public void PhysicalMinimum_ReturnsCorrect(double expected)
        {
            // set
            var header = new Signal();
            header.PhysicalMinimum = expected;

            // act
            var result = header.PhysicalMinimum;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0.123)]
        [InlineData(321.0)]
        public void PhysicalMaximum_ReturnsCorrect(double expected)
        {
            // set
            var header = new Signal();
            header.PhysicalMaximum = expected;

            // act
            var result = header.PhysicalMaximum;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(123)]
        [InlineData(321)]
        public void DigitalMinimum_ReturnsCorrect(short expected)
        {
            // set
            var header = new Signal();
            header.DigitalMinimum = expected;

            // act
            var result = header.DigitalMinimum;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(123)]
        [InlineData(321)]
        public void DigitalMaximum_ReturnsCorrect(short expected)
        {
            // set
            var header = new Signal();
            header.DigitalMaximum = expected;

            // act
            var result = header.DigitalMaximum;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("321")]
        public void Prefiltering_ReturnsCorrect(string expected)
        {
            // set
            var header = new Signal();
            header.Prefiltering = expected;

            // act
            var result = header.Prefiltering;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("321")]
        public void Reserved_ReturnsCorrect(string expected)
        {
            // set
            var header = new Signal();
            header.Reserved = expected;

            // act
            var result = header.Reserved;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(123)]
        [InlineData(321)]
        public void NumberOfSamples_ReturnsCorrect(int expected)
        {
            // set
            var header = new Signal();
            header.NumberOfSamples = expected;

            // act
            var result = header.NumberOfSamples;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ScanID_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("ScanID", typeof(Signal)));
        }

        [Fact]
        public void Channel_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Channel", typeof(Signal)));
        }

        [Fact]
        public void Label_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Label", typeof(Signal)));
        }

        [Fact]
        public void TransducerType_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("TransducerType", typeof(Signal)));
        }

        [Fact]
        public void PhysicalDimension_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("PhysicalDimension", typeof(Signal)));
        }

        [Fact]
        public void PhysicalMinimum_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("PhysicalMinimum", typeof(Signal)));
        }

        [Fact]
        public void PhysicalMaximum_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("PhysicalMaximum", typeof(Signal)));
        }

        [Fact]
        public void DigitalMinimum_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("DigitalMinimum", typeof(Signal)));
        }

        [Fact]
        public void DigitalMaximum_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("DigitalMaximum", typeof(Signal)));
        }

        [Fact]
        public void Prefiltering_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Prefiltering", typeof(Signal)));
        }

        [Fact]
        public void Reserved_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Reserved", typeof(Signal)));
        }

        [Fact]
        public void NumberOfSamples_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("NumberOfSamples", typeof(Signal)));
        }
    }
}
