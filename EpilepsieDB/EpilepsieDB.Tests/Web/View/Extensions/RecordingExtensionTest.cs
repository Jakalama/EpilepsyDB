using EpilepsieDB.Models;
using EpilepsieDB.Web.View.Extensions;
using EpilepsieDB.Web.View.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.View.Extensions
{
    public class RecordingExtensionTest : AbstractTest
    {
        public RecordingExtensionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToDto_ReturnsNotNull()
        {
            // set
            Recording rec = new Recording();

            // act
            RecordingDto dto = rec.ToDto();

            // assert
            Assert.NotNull(dto);
        }

        [Fact]
        public void ToDto()
        {
            // set
            Recording rec = new Recording()
            {
                ID = 1,
                PatientID = 1,
                Patient = new Patient()
                {
                    ID = 1,
                    Acronym = "Olaf"
                },
                RecordingNumber = "Rec001"
            };

            // act
            RecordingDto dto = rec.ToDto();

            // assert
            Assert.Equal(rec.ID, dto.RecordingID);
            Assert.Equal(rec.PatientID, dto.PatientID);
            Assert.Equal(rec.Patient.Acronym, dto.PatientAcronym);
            Assert.Equal(rec.RecordingNumber, dto.RecordingNumber);
        }

        [Fact]
        public void ToDto_SetScans()
        {
            // set
            Recording rec = new Recording()
            {
                ID = 1,
                PatientID = 1,
                Patient = new Patient()
                {
                    ID = 1,
                    Acronym = "Olaf"
                },
                RecordingNumber = "Rec001"
            };

            IEnumerable<ScanDto> scans = new List<ScanDto>()
            {
                new ScanDto()
                {
                    ScanID = 999,
                    ScanNumber = "Sc001"
                }
            };

            // act
            RecordingDto dto = rec.ToDto(scans);

            // assert
            Assert.Equal(scans.Count(), dto.Scans.Count());
            Assert.Equal(scans.First().ScanNumber, dto.Scans.First().ScanNumber);
            Assert.Equal(scans.First().ScanID, dto.Scans.First().ScanID);
        }

        [Fact]
        public void ToModel_ReturnsNotNull()
        {
            // set
            RecordingDto dto = new RecordingDto();

            // act
            Recording rec = dto.ToModel();

            // assert
            Assert.NotNull(rec);
        }

        [Fact]
        public void ToModel()
        {
            // set
            RecordingDto dto = new RecordingDto()
            {
                RecordingID = 1,
                PatientID = 1,
                PatientAcronym = "A",
                RecordingNumber = "Rec001"
            };

            // act
            Recording rec = dto.ToModel();

            // assert
            Assert.Equal(dto.RecordingID, rec.ID);
            Assert.Equal(dto.PatientID, rec.PatientID);
            Assert.Equal(dto.RecordingNumber, rec.RecordingNumber);
        }

        [Fact]
        public void ToModel_SetsPatient()
        {
            // set
            RecordingDto dto = new RecordingDto()
            {
                RecordingID = 1,
                PatientID = 1,
                PatientAcronym = "A",
                RecordingNumber = "Rec001"
            };

            Patient patient = new Patient()
            {
                Acronym = "B",
            };

            // act
            Recording rec = dto.ToModel(patient);

            // assert
            Assert.Equal(patient.Acronym, rec.Patient.Acronym);
        }

        [Fact]
        public void ToDetailDto_ReturnsNotNull()
        {
            // set
            Recording rec = new Recording();
            IEnumerable<ScanDto> scans = new List<ScanDto>();

            // act
            RecordingDetailDto dto = rec.ToDetailDto(scans);

            Assert.NotNull(dto);
        }

        [Fact]
        public void ToDetailDto()
        {
            // set
            Recording rec = new Recording()
            {
                ID = 1,
                PatientID = 1,
                RecordingNumber = "Rec001",
                Patient = new Patient()
                {
                    Acronym = "B",
                }
            };

            IEnumerable<ScanDto> scans = new List<ScanDto>()
            {
                new ScanDto()
                {
                    ScanID = 1,
                    ScanNumber = "Sc001"
                }
            };

            // act
            RecordingDetailDto dto = rec.ToDetailDto(scans);

            Assert.Equal(rec.ID, dto.RecordingID);
            Assert.Equal(rec.PatientID, dto.PatientID);
            Assert.Equal(rec.RecordingNumber, dto.RecordingNumber);
            Assert.Equal(rec.Patient.Acronym, dto.PatientAcronym);
            Assert.Equal(scans.Count(), dto.Scans.Count());
            Assert.Equal(scans.First().ScanNumber, dto.Scans.First().ScanNumber);
        }

        [Fact]
        public void ToDtos_ReturnsNotNull()
        {
            // set
            IEnumerable<Recording> recs = new List<Recording>();

            // act
            IEnumerable<RecordingDto> dtos = recs.ToDtos();

            // assert
            Assert.NotNull(dtos);
        }

        [Fact]
        public void ToDtos()
        {
            // set
            IEnumerable<Recording> recs = new List<Recording>()
            {
                new Recording()
                {
                    RecordingNumber = "Rec001"
                },
                new Recording()
                {
                    RecordingNumber = "Rec002"
                }
            };

            // act
            IEnumerable<RecordingDto> dtos = recs.ToDtos();

            // assert
            Assert.Equal(recs.Count(), dtos.Count());
            Assert.Equal(recs.First().RecordingNumber, dtos.First().RecordingNumber);
        }
    }
}
