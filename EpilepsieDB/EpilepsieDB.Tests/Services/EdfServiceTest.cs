using EpilepsieDB.EDF;
using EpilepsieDB.Models;
using EpilepsieDB.Services.Impl;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Services
{
    [Collection("Sequential")]
    public class EdfServiceTest : AbstractTest
    {
        private readonly string testFile;

        public EdfServiceTest(ITestOutputHelper output) : base(output)
        {
            // using a dynamically loaded test file will result in a false coverage calculation
            // defining a hard coded path to test file resolves this error?!
            string workingDir = AppContext.BaseDirectory;
            testFile = Directory.GetParent(workingDir).Parent.FullName;
            testFile = Directory.GetParent(testFile).Parent.FullName;

            testFile = Path.Combine(testFile, "testData", "test1.edf");
        }

        [Fact]
        public void Constructor()
        {
            // act
            var service = new EdfService();
        }

        [Fact]
        public void ReadFile_ReturnsEdfFile()
        {
            // set
            var service = new EdfService();

            // act
            var result = service.ReadFile(testFile);

            // assert
            Assert.NotNull(result);
            Assert.IsType<EdfFile>(result);
        }

        [Fact]
        public void ReadFile_EdfFileContainsHeader()
        {
            // set
            var service = new EdfService();

            // act
            var result = service.ReadFile(testFile);

            // assert
            Assert.NotNull(result.Header);
            Assert.IsType<EdfHeader>(result.Header);
        }

        [Fact]
        public void ReadFile_EdfFileContainsSignals()
        {
            // set
            var service = new EdfService();

            // act
            var result = service.ReadFile(testFile);

            // assert
            Assert.NotNull(result.Signals);
            Assert.IsType<EdfSignal[]>(result.Signals);
        }

        [Fact]
        public void ReadFile_EdfFileContainsBlockOffsets()
        {
            // set
            var service = new EdfService();

            // act
            var result = service.ReadFile(testFile);

            // assert
            Assert.NotNull(result.BlockOffsets);
            Assert.IsType<float[]>(result.BlockOffsets);
        }

        [Fact]
        public void ReadFile_EdfFileContainsAnnotations()
        {
            // set
            var service = new EdfService();

            // act
            var result = service.ReadFile(testFile);

            // assert
            Assert.NotNull(result.Annotations);
            Assert.IsType<EdfAnnotation[]>(result.Annotations);
        }

        [Fact]
        public void WriteToScan_WritesVersion_ToScan()
        {
            // set
            var expected = "0";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.Version);
            Assert.Equal(expected, scan.Version);
        }

        [Fact]
        public void WriteToScan_WritesPatientInfo_ToScan()
        {
            // set
            var expected = "X X X X";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.PatientInfo);
            Assert.Equal(expected, scan.PatientInfo);
        }

        [Fact]
        public void WriteToScan_WritesRecordInfo_ToScan()
        {
            // set
            var expected = "Startdate 12-AUG-2009 X X BCI2000";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.RecordInfo);
            Assert.Equal(expected, scan.RecordInfo);
        }

        [Fact]
        public void WriteToScan_WritesStartDate_ToScan()
        {
            // set
            var expected = "12.08.2009";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.StartDate);
            Assert.Equal(expected, scan.StartDate.ToShortDateString());
        }

        [Fact]
        public void WriteToScan_WritesStartTime_ToScan()
        {
            // set
            var expected = "16:15";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.StartTime);
            Assert.Equal(expected, scan.StartTime.ToShortTimeString());
        }

        [Fact]
        public void WriteToScan_WritesNumberOfRecords_ToScan()
        {
            // set
            var expected = 61;
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.NumberOfRecords);
            Assert.Equal(expected, scan.NumberOfRecords);
        }

        [Fact]
        public void WriteToScan_WritesDurationOfDataRecord_ToScan()
        {
            // set
            var expected = 1;
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.DurationOfDataRecord);
            Assert.Equal(expected, scan.DurationOfDataRecord);
        }

        [Fact]
        public void WriteToScan_WritesNumberOfSignals_ToScan()
        {
            // set
            var expected = 65;
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.NumberOfSignals);
            Assert.Equal(expected, scan.NumberOfSignals);
        }

        [Fact]
        public void WriteToScan_WritesLabels_ToScan()
        {
           // set
           var expected = "Fc5.;Fc3.;Fc1.;Fcz.;Fc2.;Fc4.;Fc6.;C5..;C3..;C1..;Cz..;C2..;C4..;C6..;Cp5.;Cp3.;Cp1.;Cpz.;Cp2.;Cp4.;Cp6.;Fp1.;Fpz.;Fp2.;Af7.;Af3.;Afz.;Af4.;Af8.;F7..;F5..;F3..;F1..;Fz..;F2..;F4..;F6..;F8..;Ft7.;Ft8.;T7..;T8..;T9..;T10.;Tp7.;Tp8.;P7..;P5..;P3..;P1..;Pz..;P2..;P4..;P6..;P8..;Po7.;Po3.;Poz.;Po4.;Po8.;O1..;Oz..;O2..;Iz..;EDF Annotations";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.Labels);
            Assert.Equal(expected, scan.Labels);
        }

        [Fact]
        public void WriteToScan_WritesLabels_AndDeletesDuplicates()
        {
            // set
            var expected = "Fc5.;Fc3.;Fc1.;Fcz.;Fc2.;Fc4.;Fc6.;C5..;C3..;C1..;Cz..;C2..;C4..;C6..;Cp5.;Cp3.;Cp1.;Cpz.;Cp2.;Cp4.;Cp6.;Fp1.;Fpz.;Fp2.;Af7.;Af3.;Afz.;Af4.;Af8.;F7..;F5..;F3..;F1..;Fz..;F2..;F4..;F6..;F8..;Ft7.;Ft8.;T7..;T8..;T9..;T10.;Tp7.;Tp8.;P7..;P5..;P3..;P1..;Pz..;P2..;P4..;P6..;P8..;Po7.;Po3.;Poz.;Po4.;Po8.;O1..;Oz..;O2..;Iz..;EDF Annotations";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.Labels);
            Assert.Equal(expected, scan.Labels);
        }

        [Fact]
        public void WriteToScan_WritesTransducerTypes_ToScan()
        {
            // set
            var expected = "BCI2000;";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.TransducerTypes);
            Assert.Equal(expected, scan.TransducerTypes);
        }

        [Fact]
        public void WriteToScan_WritesTransducerTypes_AndDeletesDuplicates()
        {
            // set
            var expected = "BCI2000;";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.TransducerTypes);
            Assert.Equal(expected, scan.TransducerTypes);
        }

        [Fact]
        public void WriteToScan_WritesPhysicalDimensions_ToScan()
        {
            // set
            var expected = "uV;-";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.PhysicalDimensions);
            Assert.Equal(expected, scan.PhysicalDimensions);
        }

        [Fact]
        public void WriteToScan_WritesPhysicalDimensions_AndDeletesDuplicates()
        {
            // set
            var expected = "uV;-";
            var service = new EdfService();
            var scan = new Scan();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.PhysicalDimensions);
            Assert.Equal(expected, scan.PhysicalDimensions);
        }

        [Fact]
        public void WriteToScan_WritesAnnotations_ToScan()
        {
            // set
            var scan = new Scan();
            var service = new EdfService();

            // act
            service.WriteToScan(scan, testFile);

            // assert
            Assert.NotNull(scan.Annotations);
        }
    }
}
