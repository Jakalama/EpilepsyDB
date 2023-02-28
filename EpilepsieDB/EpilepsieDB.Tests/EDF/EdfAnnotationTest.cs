using EpilepsieDB.EDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.EDF
{
    public class EdfAnnotationTest : AbstractTest
    {
        public EdfAnnotationTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void IsOnset_ReturnsCorrect()
        {
            // set
            var expected = true;
            var annotation = new EdfAnnotation();
            annotation.IsOnset = expected;

            // act
            var result = annotation.IsOnset;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void StartOffset_ReturnsCorrect()
        {
            // set
            var expected = 123;
            var annotation = new EdfAnnotation();
            annotation.StartOffset = expected;

            // act
            var result = annotation.StartOffset;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Duration_ReturnsCorrect()
        {
            // set
            var expected = 0.34f;
            var annotation = new EdfAnnotation();
            annotation.Duration = expected;

            // act
            var result = annotation.Duration;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Description_ReturnsCorrect()
        {
            // set
            var expected = "test";
            var annotation = new EdfAnnotation();
            annotation.Description = expected;

            // act
            var result = annotation.Description;

            // assert
            Assert.Equal(expected, result);
        }
    }
}
