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
    public class EdfSignalTest : AbstractTest
    {
        public EdfSignalTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Samples_ReturnsCorrect()
        {
            // set
            var expected = new List<short> { 1, 2, 3 };
            var signal = new EdfSignal();
            signal.Samples = expected;

            // act
            var result = signal.Samples;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Samples_AreInitialized()
        {
            // set
            var signal = new EdfSignal();

            // act
            var result = signal.Samples;

            // assert
            Assert.NotNull(result);
        }
    }
}
