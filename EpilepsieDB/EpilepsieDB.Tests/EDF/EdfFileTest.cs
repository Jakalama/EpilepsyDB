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
    public class EdfFileTest : AbstractTest
    {
        public EdfFileTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_CreatesHeader()
        {
            // act
            var edf = new EdfFile();

            // assert
            Assert.NotNull(edf.Header);
        }

        [Fact]
        public void Constructor_CreatesSignals()
        {
            // act
            var edf = new EdfFile();

            // assert
            Assert.NotNull(edf.Signals);
        }

        [Fact]
        public void Constructor_CreatesBlockOffsets()
        {
            // act
            var edf = new EdfFile();

            // assert
            Assert.NotNull(edf.BlockOffsets);
        }

        [Fact]
        public void Constructor_CreatesAnnotations()
        {
            // act
            var edf = new EdfFile();

            // assert
            Assert.NotNull(edf.Annotations);
        }

        [Fact]
        public void Constructor_SetsHeader()
        {
            // act
            var header = new EdfHeader();
            var edf = new EdfFile(header, null, null, null);

            // assert
            Assert.NotNull(edf.Header);
            Assert.Equal(header, edf.Header);
        }

        [Fact]
        public void Constructor_SetsBlockOffsets()
        {
            // act
            var offsets = new float[] { 0f };
            var edf = new EdfFile(null, offsets, null, null);

            // assert
            Assert.NotNull(edf.BlockOffsets);
            Assert.Equal(offsets, edf.BlockOffsets);
        }

        [Fact]
        public void Constructor_SetsSignals()
        {
            // act
            var signals = new EdfSignal[] { new EdfSignal() };
            var edf = new EdfFile(null, null, signals, null);

            // assert
            Assert.NotNull(edf.Signals);
            Assert.Equal(signals, edf.Signals);
        }

        [Fact]
        public void Constructor_SetsAnnotations()
        {
            // act
            var annotations = new EdfAnnotation[] { new EdfAnnotation() };
            var edf = new EdfFile(null, null, null, annotations);

            // assert
            Assert.NotNull(edf.Annotations);
            Assert.Equal(annotations, edf.Annotations);
        }

        [Fact]
        public void Header_ReturnsCorrect()
        {
            // set
            var expected = new EdfHeader();
            var file = new EdfFile();
            file.Header = expected;

            // act
            var result = file.Header;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BlockOffsets_ReturnsCorrect()
        {
            // set
            var expected = new float[0];
            var file = new EdfFile();
            file.BlockOffsets = expected;

            // act
            var result = file.BlockOffsets;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Signals_ReturnsCorrect()
        {
            // set
            var expected = new EdfSignal[0];
            var file = new EdfFile();
            file.Signals = expected;

            // act
            var result = file.Signals;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Annotations_ReturnsCorrect()
        {
            // set
            var expected = new EdfAnnotation[0];
            var file = new EdfFile();
            file.Annotations = expected;

            // act
            var result = file.Annotations;

            // assert
            Assert.Equal(expected, result);
        }
    }
}
