using EpilepsieDB.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Models
{
    public class AnnotationTest : AbstractTest
    {
        public AnnotationTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(123)]
        [InlineData(321)]
        public void ScanID_ReturnsCorrect(int expected)
        {
            // set
            var annotation = new Annotation();
            annotation.ScanID = expected;

            // act
            var result = annotation.ScanID;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Scan_ReturnsCorrect()
        {
            // set
            var expected = new Scan();
            var annotation = new Annotation();
            annotation.Scan = expected;

            // act
            var result = annotation.Scan;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0.123)]
        [InlineData(3.210)]
        public void Offset_ReturnsCorrect(float expected)
        {
            // set
            var annotation = new Annotation();
            annotation.Offset = expected;

            // act
            var result = annotation.Offset;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0.123)]
        [InlineData(3.210)]
        public void Duration_ReturnsCorrect(float expected)
        {
            // set
            var annotation = new Annotation();
            annotation.Duration = expected;

            // act
            var result = annotation.Duration;

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("case")]
        public void Description_ReturnsCorrect(string expected)
        {
            // set
            var annotation = new Annotation();
            annotation.Description = expected;

            // act
            var result = annotation.Description;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ScanID_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("ScanID", typeof(Annotation)));
        }

        [Fact]
        public void Offset_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Offset", typeof(Annotation)));
        }

        [Fact]
        public void Duration_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Duration", typeof(Annotation)));
        }

        [Fact]
        public void Description_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Description", typeof(Annotation)));
        }
    }
}
