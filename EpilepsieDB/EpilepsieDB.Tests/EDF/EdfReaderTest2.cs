using EpilepsieDB.EDF;
using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.EDF
{
    [Collection("Sequential")]
    public class EdfReaderTest2 : AbstractTest
    {
        private string testFile;

        public EdfReaderTest2(ITestOutputHelper output) : base(output)
        {
            // using a dynamically loaded test file will result in a false coverage calculation
            // defining a hard coded path to test file resolves this error?!
            string workingDir = AppContext.BaseDirectory;
            testFile = Directory.GetParent(workingDir).Parent.FullName;
            testFile = Directory.GetParent(testFile).Parent.FullName;

            testFile = Path.Combine(testFile, "testData", "test2.edf");
        }

        [Fact]
        public void HeaderContainsVersion()
        {
            // set
            string expected = "0";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.Version.Value);
        }

        [Fact]
        public void HeaderContainsPatientID()
        {
            // set
            string expected = "X X X X";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.PatientID.Value);
        }

        [Fact]
        public void HeaderContainsRecordID()
        {
            // set
            string expected = "Startdate 12-AUG-1995 X X BCI2000";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.RecordID.Value);
        }

        [Fact]
        public void HeaderContainsStartDate()
        {
            // set
            string expected = "12.08.1995";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.StartDate.Value);
        }

        [Fact]
        public void HeaderContainsStartTime()
        {
            // set
            string expected = "16.15.00";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.StartTime.Value);
        }

        [Fact]
        public void HeaderContainsNumberOfBytesInHeader()
        {
            // set
            short expected = 16896;

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.NumberOfBytesInHeader.Value);
        }

        [Fact]
        public void HeaderContainsReserved()
        {
            // set
            string expected = "EDF+D";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.Reserved.Value);
        }

        [Fact]
        public void HeaderContainsNumberOfDataRecords()
        {
            // set
            short expected = 61;

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.NumberOfDataRecords.Value);
        }

        [Fact]
        public void HeaderContainsDurationOfDataRecord()
        {
            // set
            short expected = 1;

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.DurationOfDataRecord.Value);
        }

        [Fact]
        public void HeaderContainsNumberOfSignals()
        {
            // set
            short expected = 65;

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.NumberOfSignals.Value);
        }

        [Fact]
        public void HeaderContainsCorrectAmountOfSignals()
        {
            // set
            int expected = 65;

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders.Length);
        }

        [Fact]
        public void FirstLabelContainsString()
        {
            // set
            string expected = "Fc5.";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders[0].Label.Value);
        }

        [Fact]
        public void FirstTransducerTypeContainsString()
        {
            // set
            string expected = "BCI2000";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders[0].TransducerType.Value);
        }

        [Fact]
        public void FirstPhysicalDimensionContainsString()
        {
            // set
            string expected = "uV";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders[0].PhysicalDimension.Value);
        }

        [Fact]
        public void FirstPhysicalMinimumContainsString()
        {
            // set
            double expected = -8092;

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders[0].PhysicalMinimum.Value);
        }

        [Fact]
        public void FirstPhysicalMaximumContainsString()
        {
            // set
            double expected = 8092;

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders[0].PhysicalMaximum.Value);
        }

        [Fact]
        public void FirstDigitalMinimumContainsString()
        {
            // set
            short expected = -8092;

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders[0].DigitalMinimum.Value);
        }

        [Fact]
        public void FirstDigitalMaximumContainsString()
        {
            // set
            short expected = 8092;

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders[0].DigitalMaximum.Value);
        }

        [Fact]
        public void FirstPrefilteringContainsString()
        {
            // set
            string expected = "HP:0Hz LP:0Hz N:0Hz";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders[0].Prefiltering.Value);
        }

        [Fact]
        public void FirstNumberOfSamplesContainsString()
        {
            // set
            short expected = 160;

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders[0].NumberOfSamples.Value);
        }

        [Fact]
        public void FirstSignalsReservedContainsString()
        {
            // set
            string expected = "";

            // act
            var service = new EdfReader(testFile);
            var edf = service.ReadHeader();
            service.Close();

            // assert
            Assert.Equal(expected, edf.SignalHeaders[0].Reserved.Value);
        }

        [Fact]
        public void SignalsContainsSampleData()
        {
            // act
            var service = new EdfReader(testFile);
            var header = service.ReadHeader();
            var edf = service.ReadSignals(header);
            service.Close();

            // assert
            Assert.NotNull(edf);
            Assert.NotNull(edf[0].Samples);
        }

        [Fact]
        public void SignalsHaveCorrectNumberOfSamples()
        {
            // set
            int expected = 19520;

            // act
            var service = new EdfReader(testFile);
            var header = service.ReadHeader();
            var edf = service.ReadSignals(header);
            service.Close();

            // assert
            Assert.Equal(expected, edf[0].Samples.Count());
        }
    }
}
