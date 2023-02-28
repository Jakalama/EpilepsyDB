using EpilepsieDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Models
{
    public class ScanTest : AbstractTest
    {
        public ScanTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ID_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = 1;

            // act
            scan.ID = expected;

            // assert
            Assert.Equal(expected, scan.ID);
        }

        [Fact]
        public void RecordingID_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = 1;

            // act
            scan.RecordingID = expected;

            // assert
            Assert.Equal(expected, scan.RecordingID);
        }

        [Fact]
        public void Recording_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = new Recording();

            // act
            scan.Recording = expected;

            // assert
            Assert.Equal(expected, scan.Recording);
        }

        [Fact]
        public void EdfDisplayName_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = "test";

            // act
            scan.EdfDisplayName = expected;

            // assert
            Assert.Equal(expected, scan.EdfDisplayName);
        }

        [Fact]
        public void EdfFilePath_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = "test";

            // act
            scan.EdfFilePath = expected;

            // assert
            Assert.Equal(expected, scan.EdfFilePath);
        }

        [Fact]
        public void ScanNumber_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = "test";

            // act
            scan.ScanNumber = expected;

            // assert
            Assert.Equal(expected, scan.ScanNumber);
        }

        [Fact]
        public void Version_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = "test";

            // act
            scan.Version = expected;

            // assert
            Assert.Equal(expected, scan.Version);
        }

        [Fact]
        public void PatientInfo_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = "test";

            // act
            scan.PatientInfo = expected;

            // assert
            Assert.Equal(expected, scan.PatientInfo);
        }

        [Fact]
        public void RecordInfo_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = "test";

            // act
            scan.RecordInfo = expected;

            // assert
            Assert.Equal(expected, scan.RecordInfo);
        }

        [Fact]
        public void StartDate_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = new DateTime();

            // act
            scan.StartDate = expected;

            // assert
            Assert.Equal(expected, scan.StartDate);
        }

        [Fact]
        public void StartTime_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = new DateTime();

            // act
            scan.StartTime = expected;

            // assert
            Assert.Equal(expected, scan.StartTime);
        }

        [Fact]
        public void NumberOfRecords_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = 1;

            // act
            scan.NumberOfRecords = expected;

            // assert
            Assert.Equal(expected, scan.NumberOfRecords);
        }

        [Fact]
        public void DurationOfDataRecord_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = 1;

            // act
            scan.DurationOfDataRecord = expected;

            // assert
            Assert.Equal(expected, scan.DurationOfDataRecord);
        }

        [Fact]
        public void NumberOfSignals_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = 1;

            // act
            scan.NumberOfSignals = expected;

            // assert
            Assert.Equal(expected, scan.NumberOfSignals);
        }

        [Fact]
        public void Labels_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = "test";

            // act
            scan.Labels = expected;

            // assert
            Assert.Equal(expected, scan.Labels);
        }

        [Fact]
        public void TransducerTypes_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = "test";

            // act
            scan.TransducerTypes = expected;

            // assert
            Assert.Equal(expected, scan.TransducerTypes);
        }

        [Fact]
        public void PhysicalDimensions_GetSet()
        {
            // set
            var scan = new Scan();
            var expected = "test";

            // act
            scan.PhysicalDimensions = expected;

            // assert
            Assert.Equal(expected, scan.PhysicalDimensions);
        }

        [Fact]
        public void Blocks_ReturnsCorrect()
        {
            // set
            var expected = new List<Block>();
            var scan = new Scan();
            scan.Blocks = expected;

            // act
            var result = scan.Blocks;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Signals_ReturnsCorrect()
        {
            // set
            var expected = new List<Signal>();
            var scan = new Scan();
            scan.Signals = expected;

            // act
            var result = scan.Signals;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Annotations_ReturnsCorrect()
        {
            // set
            var expected = new List<Annotation>();
            var scan = new Scan();
            scan.Annotations = expected;

            // act
            var result = scan.Annotations;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void RecordingID_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("RecordingID", typeof(Scan)));
        }

        [Fact]
        public void EdfDisplayName_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("EdfDisplayName", typeof(Scan)));
        }

        [Fact]
        public void EdfFilePath_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("EdfFilePath", typeof(Scan)));
        }

        [Fact]
        public void ScanNumber_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("ScanNumber", typeof(Scan)));
        }

        [Fact]
        public void Version_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Version", typeof(Scan)));
        }

        [Fact]
        public void PatientInfo_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("PatientInfo", typeof(Scan)));
        }

        [Fact]
        public void RecordInfo_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("RecordInfo", typeof(Scan)));
        }

        [Fact]
        public void StartDate_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("StartDate", typeof(Scan)));
        }

        [Fact]
        public void StartTime_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("StartTime", typeof(Scan)));
        }

        [Fact]
        public void NumberOfRecords_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("NumberOfRecords", typeof(Scan)));
        }

        [Fact]
        public void DurationOfDataRecord_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("DurationOfDataRecord", typeof(Scan)));
        }

        [Fact]
        public void NumberOfSignals_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("NumberOfSignals", typeof(Scan)));
        }

        [Fact]
        public void Labels_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Labels", typeof(Scan)));
        }

        [Fact]
        public void TransducerTypes_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("TransducerTypes", typeof(Scan)));
        }

        [Fact]
        public void PhysicalDimensions_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("PhysicalDimensions", typeof(Scan)));
        }

        [Fact]
        public void Blocks_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Blocks", typeof(Scan)));
        }

        [Fact]
        public void Signals_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Signals", typeof(Scan)));
        }

        [Fact]
        public void Annotations_IsRequired()
        {
            Assert.True(Helper.Helper.PropertyHasAttribute<RequiredAttribute>("Annotations", typeof(Scan)));
        }
    }
}
