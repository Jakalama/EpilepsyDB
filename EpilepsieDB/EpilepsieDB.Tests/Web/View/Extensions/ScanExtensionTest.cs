using EpilepsieDB.Models;
using EpilepsieDB.Web.View.Extensions;
using EpilepsieDB.Web.View.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.View.Extensions
{
    public class ScanExtensionTest : AbstractTest
    {
        public ScanExtensionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToDto()
        {
            int id = 1;
            int patientID = 123;
            string filename = "Test.txt";

            // set
            Scan scan = new Scan()
            {
                ID = id,
                RecordingID = patientID,
                ScanNumber = filename
            };

            // act
            ScanDto dto = scan.ToDto();

            // assert
            Assert.Equal(id, dto.ScanID);
            Assert.Equal(patientID, dto.RecordingID);
            Assert.Equal(filename, dto.ScanNumber);
        }

        [Fact]
        public void ToDetailDto()
        {
            int id = 1;
            int patientID = 123;
            string filename = "Test.txt";

            // set
            Scan scan = new Scan()
            {
                ID = id,
                RecordingID = patientID,
                ScanNumber = filename
            };

            // act
            ScanDetailDto dto = scan.ToDetailDto();

            // assert
            Assert.Equal(id, dto.ScanID);
            Assert.Equal(patientID, dto.RecordingID);
            Assert.Equal(filename, dto.ScanNumber);
        }

        [Fact]
        public void ToDetailDto_WithSignals()
        {
            int id = 1;
            int patientID = 123;
            string filename = "Test.txt";

            // set
            Scan scan = new Scan()
            {
                ID = id,
                RecordingID = patientID,
                ScanNumber = filename,
                DurationOfDataRecord = 1,
            };

            var label = "Test";
            var type = "Test Type";
            var gain = 0.04884f;
            var samples = 60;
            var rate = 60;
            var signals = new List<Signal>()
            {
                new Signal()
                {
                    Scan = scan,
                    ScanID = scan.ID,
                    Channel = 1,
                    Label = label,
                    TransducerType = type,
                    PhysicalMaximum = 100,
                    PhysicalMinimum = 0,
                    DigitalMaximum = 1023,
                    DigitalMinimum = -1024,
                    NumberOfSamples = samples
                }
            };

            // act
            ScanDetailDto dto = scan.ToDetailDto(signals);

            // assert
            Assert.NotNull(dto.Electrodes);
            Assert.Equal(1, dto.Electrodes.Count());
            Assert.Equal(label, dto.Electrodes.First().Label);
            Assert.Equal(type, dto.Electrodes.First().Type);
            Assert.Equal(gain, dto.Electrodes.First().Gain, 0.00001);
            Assert.Equal(samples, dto.Electrodes.First().Samples);
            Assert.Equal(rate, dto.Electrodes.First().SamplingRate);
        }

        [Fact]
        public void ToDetailDto_WithEdfSignal()
        {
            int id = 1;
            int patientID = 123;
            string filename = "Test.txt";

            // set
            Scan scan = new Scan()
            {
                ID = id,
                RecordingID = patientID,
                ScanNumber = filename,
                DurationOfDataRecord = 1,
            };

            var label = "Edf annotations";
            var signals = new List<Signal>()
            {
                new Signal()
                {
                    Scan = scan,
                    ScanID = scan.ID,
                    Label = label,
                }
            };

            // act
            ScanDetailDto dto = scan.ToDetailDto(signals);

            // assert
            Assert.NotNull(dto.Electrodes);
            Assert.Equal(0, dto.Electrodes.Count());
        }

        [Fact]
        public void ToDetailDto_WithSignalsAndBlocks()
        {
            int id = 1;
            int patientID = 123;
            string filename = "Test.txt";

            // set
            Scan scan = new Scan()
            {
                ID = id,
                RecordingID = patientID,
                ScanNumber = filename,
                DurationOfDataRecord = 1,
            };

            var label = "Test";
            var type = "Test Type";
            var gain = 0.04884f;
            var samples = 60;
            var rate = 60;
            var signals = new List<Signal>()
            {
                new Signal()
                {
                    Scan = scan,
                    ScanID = scan.ID,
                    Channel = 1,
                    Label = label,
                    TransducerType = type,
                    PhysicalMaximum = 100,
                    PhysicalMinimum = 0,
                    DigitalMaximum = 1023,
                    DigitalMinimum = -1024,
                    NumberOfSamples = samples
                }
            };

            var start = DateTime.Now;
            var end = DateTime.Now.AddSeconds(5);
            var gap = 1;
            var blocks = new List<Block>()
            {
                new Block()
                {
                    Starttime = start,
                    Endtime = end,
                    GapToPrevious = gap
                }
            };

            // act
            ScanDetailDto dto = scan.ToDetailDto(signals, blocks);

            // assert
            Assert.NotNull(dto.Blocks);
            Assert.Equal(1, dto.Blocks.Count());
            Assert.Equal(start, dto.Blocks.First().Starttime);
            Assert.Equal(end, dto.Blocks.First().Endtime);
            Assert.Equal(gap, dto.Blocks.First().Gap);
            Assert.Equal(1, dto.Blocks.First().NumChannels);
            Assert.Equal(1, dto.Blocks.First().BlockNumber);
        }

        [Fact]
        public void ToDtos()
        {
            int id = 1;
            int patientID = 123;
            string filename = "Test.txt";

            // set
            IEnumerable<Scan> list = new List<Scan>()
            {
                new Scan()
                {
                    ID = id,
                    RecordingID = patientID,
                    ScanNumber = filename
                },
                new Scan()
                {
                    ID = id,
                    RecordingID = patientID,
                    ScanNumber = filename
                }
            };

            // act
            List<ScanDto> dtos = list.ToDto().ToList();

            // assert
            Assert.Equal(2, dtos.Count);
            Assert.Equal(id, dtos[0].ScanID);
            Assert.Equal(patientID, dtos[0].RecordingID);
            Assert.Equal(filename, dtos[0].ScanNumber);
        }

        [Fact]
        public void ToModel()
        {
            int id = 1;
            int patientID = 123;
            string filename = "Test.txt";

            // set
            ScanDto dto = new ScanDto()
            {
                ScanID = id,
                RecordingID = patientID,
                ScanNumber = filename
            };

            // act
            Scan model = dto.ToModel();

            // assert
            Assert.Equal(id, model.ID);
            Assert.Equal(patientID, model.RecordingID);
            Assert.Equal(filename, model.ScanNumber);
        }
    }
}
