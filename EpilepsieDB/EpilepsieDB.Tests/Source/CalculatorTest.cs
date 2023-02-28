using EpilepsieDB.Source.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Source
{
    public class CalculatorTest : AbstractTest
    {
        public CalculatorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(1, 1, 1, 1, 0)]
        [InlineData(2, 1, 1, 1, 0)]
        [InlineData(10, 1, 10, 1, 1)]
        [InlineData(1, -1, 1, -1, 1)]
        [InlineData(10, 1, 2, 1, 9)]
        [InlineData(2, 1, 10, 1, 0.11111)]
        [InlineData(-1, 1, 2, 1, -2)]
        [InlineData(2, 1, -1, 1, -0.5)]
        public void Gain(float pmax, float pmin, float dmax, float dmin, float expected)
        {
            // act
            var res = Calculator.Gain(pmax, pmin, dmax, dmin);

            // assert
            Assert.Equal(expected, res, 0.000001);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 2)]
        [InlineData(10, 1, 10)]
        [InlineData(1, 2, 0.5)]
        [InlineData(1, 0, 0)]
        [InlineData(-1, 1, -1)]
        public void SamplingRate(float samples, float time, float expected)
        {
            // act
            var res = Calculator.SamplingRate(time, samples);

            // assert
            Assert.Equal(expected, res, 0.001);
        }
    }
}
