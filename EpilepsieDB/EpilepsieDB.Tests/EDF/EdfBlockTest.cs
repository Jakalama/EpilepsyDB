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
    public class EdfBlockTest : AbstractTest
    {
        public EdfBlockTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Offset_ReturnsCorrect()
        {
            // set
            var expeced = 123;
            var block = new EdfBlock();
            block.Offset = expeced;

            // act
            var result = block.Offset;

            // assert
            Assert.Equal(expeced, result);
        }

        [Fact]
        public void Annotations_ReturnsCorrect()
        {
            // set
            var expeced = new List<EdfAnnotation>();
            var block = new EdfBlock();
            block.Annotations = expeced;

            // act
            var result = block.Annotations;

            // assert
            Assert.Equal(expeced, result);
        }

        [Fact]
        public void Annotations_IsInitialized()
        {
            // set
            var block = new EdfBlock();

            // act
            var result = block.Annotations;

            // assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }
    }
}
