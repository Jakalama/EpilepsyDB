using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Services;
using EpilepsieDB.Services.Impl;
using EpilepsieDB.Source;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Services
{
    public class PatientServiceTest : AbstractTest
    {
        public PatientServiceTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor()
        {
            // set
            var repoMock = new Mock<IRepository<Patient>>();
            var fileServiceMock = new Mock<IFileService>();

            // act
            var service = new PatientService(repoMock.Object, fileServiceMock.Object);
        }

        [Fact]
        public async Task GetPatient_ReturnsPatient()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };
            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>())).
                ReturnsAsync(patient);
            var service = new PatientService(repoMock.Object, null);

            // act
            var result = await service.Get(It.IsAny<int>());

            // assert
            Assert.Equal(patient, result);
        }

        [Fact]
        public async Task GetPatients_ReturnsPatients()
        {
            // set
            var patients = new List<Patient>()
            {
                new Patient() { ID = 1},
                new Patient() { ID = 2}
            };
            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.GetAll())
                .ReturnsAsync(patients);
            var service = new PatientService(repoMock.Object, null);

            // act
            var result = await service.GetAll();

            // assert
            Assert.Equal(2, result.Count);
            Assert.Equal(patients, result);
        }

        [Fact]
        public async Task CreateNew_AddsNoPatient_IfFilesAreNull()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "TestPatient",
            };
            var list = new List<Patient>();

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Add(patient))
                .Callback<Patient>((p) => list.Add(p));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()))
                .Returns(patient.Acronym);
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync("filepath");
            fileServiceMock.Setup(m => m.CreateWebResource(It.IsAny<IFileStream>()))
                .ReturnsAsync("filepath");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            await service.Create(patient, null, null);

            // assert
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task CreateNew_AddsNoPatient_IfNiftiIsNull()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "TestPatient",
            };
            var list = new List<Patient>();

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Add(patient))
                .Callback<Patient>((p) => list.Add(p));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()))
                .Returns(patient.Acronym);
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync("filepath");
            fileServiceMock.Setup(m => m.CreateWebResource(It.IsAny<IFileStream>()))
                .ReturnsAsync("filepath");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            await service.Create(patient, It.IsAny<IFileStream>(), null);

            // assert
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task CreateNew_AddsNoPatient_IfMriIsNull()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "TestPatient",
            };
            var list = new List<Patient>();

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Add(patient))
                .Callback<Patient>((p) => list.Add(p));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()))
                .Returns(patient.Acronym);
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync("filepath");
            fileServiceMock.Setup(m => m.CreateWebResource(It.IsAny<IFileStream>()))
                .ReturnsAsync("filepath");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            await service.Create(patient, null, It.IsAny<IFileStream>());

            // assert
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task CreateNew_AddsPatient()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "TestPatient",
            };
            var list = new List<Patient>();

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Add(patient))
                .Callback<Patient>((p) => list.Add(p));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()))
                .Returns(patient.Acronym);
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync("filepath");
            fileServiceMock.Setup(m => m.CreateWebResource(It.IsAny<IFileStream>()))
                .ReturnsAsync("filepath");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            var niftiMock = new Mock<IFileStream>();
            var mriMock = new Mock<IFileStream>();

            // act
            await service.Create(patient, niftiMock.Object, mriMock.Object);

            // assert
            Assert.Equal(1, list.Count);
            Assert.Equal(patient, list[0]);
        }

        [Fact]
        public async Task CreateNew_CreatesNoFiles_IfFilesAreNull()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "TestPatient",
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Add(patient));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()))
                .Returns("dirName");
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync("filepath");
            fileServiceMock.Setup(m => m.CreateWebResource(It.IsAny<IFileStream>()))
                .ReturnsAsync("filepath");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            await service.Create(patient, null, null);

            // assert
            fileServiceMock.Verify(m => m.CreateDirectory(It.IsAny<string>()), Times.Never());
            fileServiceMock.Verify(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()), Times.Never());
            fileServiceMock.Verify(m => m.CreateWebResource(It.IsAny<IFileStream>()), Times.Never());
        }

        [Fact]
        public async Task CreateNew_CreatesNoFiles_IfNiftiIsNull()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "TestPatient",
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Add(patient));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()))
                .Returns("dirName");
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync("filepath");
            fileServiceMock.Setup(m => m.CreateWebResource(It.IsAny<IFileStream>()))
                .ReturnsAsync("filepath");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            var mriMock = new Mock<IFileStream>();

            // act
            await service.Create(patient, null, mriMock.Object);

            // assert
            fileServiceMock.Verify(m => m.CreateDirectory(It.IsAny<string>()), Times.Never());
            fileServiceMock.Verify(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()), Times.Never());
            fileServiceMock.Verify(m => m.CreateWebResource(It.IsAny<IFileStream>()), Times.Never());
        }

        [Fact]
        public async Task CreateNew_CreatesNoFiles_IfMriIsNull()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "TestPatient",
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Add(patient));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()))
                .Returns("dirName");
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync("filepath");
            fileServiceMock.Setup(m => m.CreateWebResource(It.IsAny<IFileStream>()))
                .ReturnsAsync("filepath");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            var niftiMock = new Mock<IFileStream>();

            // act
            await service.Create(patient, niftiMock.Object, null);

            // assert
            fileServiceMock.Verify(m => m.CreateDirectory(It.IsAny<string>()), Times.Never());
            fileServiceMock.Verify(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()), Times.Never());
            fileServiceMock.Verify(m => m.CreateWebResource(It.IsAny<IFileStream>()), Times.Never());
        }

        [Fact]
        public async Task CreateNew_CreatesContentDir()
        {
            // set
            var randomPath = "nfkjbfd";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "TestPatient",
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Add(patient));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()))
                .Returns(randomPath);
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync("filepath");
            fileServiceMock.Setup(m => m.CreateWebResource(It.IsAny<IFileStream>()))
                .ReturnsAsync("filepath");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            var niftiMock = new Mock<IFileStream>();
            var mriMock = new Mock<IFileStream>();

            // act
            await service.Create(patient, niftiMock.Object, mriMock.Object);

            // assert
            Assert.Equal(randomPath, patient.ContentDir);
            fileServiceMock.Verify(m => m.CreateDirectory(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task CreateNew_CreatesNiftiFile()
        {
            // set
            var randomPath = "nfkjbfd";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "TestPatient",
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Add(patient));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()))
                .Returns("dirName");
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync(randomPath);
            fileServiceMock.Setup(m => m.CreateWebResource(It.IsAny<IFileStream>()))
                .ReturnsAsync("filepath");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            var niftiMock = new Mock<IFileStream>();
            var mriMock = new Mock<IFileStream>();

            // act
            await service.Create(patient, niftiMock.Object, mriMock.Object);

            // assert
            Assert.Equal(randomPath, patient.NiftiFilePath);
            fileServiceMock.Verify(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task CreateNew_CreatesMriImage()
        {
            // set
            var randomPath = "nfkjbfd";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "TestPatient",
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Add(patient));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()))
                .Returns("dirName");
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync("filepath");
            fileServiceMock.Setup(m => m.CreateWebResource(It.IsAny<IFileStream>()))
                .ReturnsAsync(randomPath);

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            var niftiMock = new Mock<IFileStream>();
            var mriMock = new Mock<IFileStream>();

            // act
            await service.Create(patient, niftiMock.Object, mriMock.Object);

            // assert
            Assert.Equal(randomPath, patient.MriImagePath);
            fileServiceMock.Verify(m => m.CreateWebResource(It.IsAny<IFileStream>()), Times.Once());
        }

        [Fact]
        public async Task Update_ReturnsTrue_IfPatientExists_IfNiftiIsNull()
        {
            // set
            string updatedValue = "987";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "123",
            };
            var updatedPatient = new Patient()
            {
                ID = 1,
                Acronym = updatedValue,
            };
            var storedPatient = patient;

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Update(updatedPatient))
                .Callback<Patient>((p) => storedPatient = p);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);

            var service = new PatientService(repoMock.Object, null);

            // act
            var result = await service.Update(updatedPatient, null, null);

            // assert
            Assert.Equal(true, result);
            Assert.Equal(updatedValue, storedPatient.Acronym);
        }

        [Fact]
        public async Task Update_ReturnsFalse_IfPatientNotExists_IfNiftiIsNull()
        {
            // set
            string expectedvalue = "123";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = expectedvalue
            };
            var updatedPatient = new Patient()
            {
                ID = 2,
                Acronym = "987",
            };
            var storedPatient = patient;

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Update(updatedPatient))
                .Callback<Patient>((p) => storedPatient = p);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(false);
            var service = new PatientService(repoMock.Object, null);

            // act
            var result = await service.Update(updatedPatient, null, null);

            // assert
            Assert.Equal(false, result);
            Assert.Equal(expectedvalue, storedPatient.Acronym);
        }

        [Fact]
        public async Task Update_ReturnsTrue_IfPatientExists_WithNifti()
        {
            // set
            string updatedValue = "987";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "123",
            };
            var updatedPatient = new Patient()
            {
                ID = 1,
                Acronym = updatedValue,
            };
            var storedPatient = patient;

            var fileMock = new Mock<IFileStream>();

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Update(updatedPatient))
                .Callback<Patient>((p) => storedPatient = p);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);
            repoMock.Setup(m => m.GetNoTracking(It.IsAny<int>()))
                .ReturnsAsync(storedPatient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateFile(
                It.IsAny<IFileStream>(),
                It.IsAny<string>()))
                .ReturnsAsync("path");
            fileServiceMock.Setup(m => m.CreateWebResource(
                It.IsAny<IFileStream>()))
                .ReturnsAsync("path");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            var result = await service.Update(updatedPatient, fileMock.Object, null);

            // assert
            Assert.Equal(true, result);
            Assert.Equal(updatedValue, storedPatient.Acronym);
        }

        [Fact]
        public async Task Update_DeletesOldNiftiFile()
        {
            // set
            string updatedValue = "987";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "123",
            };
            var updatedPatient = new Patient()
            {
                ID = 1,
                Acronym = updatedValue,
            };
            var storedPatient = patient;

            var fileMock = new Mock<IFileStream>();

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Update(updatedPatient))
                .Callback<Patient>((p) => storedPatient = p);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);
            repoMock.Setup(m => m.GetNoTracking(It.IsAny<int>()))
                .ReturnsAsync(storedPatient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateFile(
                It.IsAny<IFileStream>(),
                It.IsAny<string>()))
                .ReturnsAsync("path");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            var result = await service.Update(updatedPatient, fileMock.Object, null);

            // assert
            fileServiceMock.Verify(m => m.DeleteFile(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Update_DeletesOldMriImage()
        {
            // set
            string updatedValue = "987";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "123",
            };
            var updatedPatient = new Patient()
            {
                ID = 1,
                Acronym = updatedValue,
            };
            var storedPatient = patient;

            var fileMock = new Mock<IFileStream>();

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Update(updatedPatient))
                .Callback<Patient>((p) => storedPatient = p);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);
            repoMock.Setup(m => m.GetNoTracking(It.IsAny<int>()))
                .ReturnsAsync(storedPatient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateWebResource(
                It.IsAny<IFileStream>()))
                .ReturnsAsync("path");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            var result = await service.Update(updatedPatient, null, fileMock.Object);

            // assert
            fileServiceMock.Verify(m => m.DeleteWebResource(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Update_CreatesNewNiftiFile()
        {
            // set
            string updatedValue = "987";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "123",
            };
            var updatedPatient = new Patient()
            {
                ID = 1,
                Acronym = updatedValue,
            };
            var storedPatient = patient;

            var fileMock = new Mock<IFileStream>();

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Update(updatedPatient))
                .Callback<Patient>((p) => storedPatient = p);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);
            repoMock.Setup(m => m.GetNoTracking(It.IsAny<int>()))
                .ReturnsAsync(storedPatient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateFile(
                It.IsAny<IFileStream>(),
                It.IsAny<string>()))
                .ReturnsAsync("path");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            var result = await service.Update(updatedPatient, fileMock.Object, null);

            // assert
            fileServiceMock.Verify(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Update_CreatesNewMriImage()
        {
            // set
            string updatedValue = "987";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "123",
            };
            var updatedPatient = new Patient()
            {
                ID = 1,
                Acronym = updatedValue,
            };
            var storedPatient = patient;

            var fileMock = new Mock<IFileStream>();

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Update(updatedPatient))
                .Callback<Patient>((p) => storedPatient = p);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);
            repoMock.Setup(m => m.GetNoTracking(It.IsAny<int>()))
                .ReturnsAsync(storedPatient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateWebResource(
                It.IsAny<IFileStream>()))
                .ReturnsAsync("path");

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            var result = await service.Update(updatedPatient, null, fileMock.Object);

            // assert
            fileServiceMock.Verify(m => m.CreateWebResource(It.IsAny<IFileStream>()), Times.Once());
        }

        [Fact]
        public async Task Update_ReturnsFalse_IfPatientNotExists()
        {
            // set
            string expectedvalue = "123";
            var patient = new Patient()
            {
                ID = 1,
                Acronym = expectedvalue
            };
            var updatedPatient = new Patient()
            {
                ID = 2,
                Acronym = "987",
            };
            var storedPatient = patient;

            var niftiMock = new Mock<IFileStream>();
            var mriMock = new Mock<IFileStream>();

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Update(updatedPatient))
                .Callback<Patient>((p) => storedPatient = p);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(false);
            var service = new PatientService(repoMock.Object, null);

            // act
            var result = await service.Update(updatedPatient, niftiMock.Object, mriMock.Object);

            // assert
            Assert.Equal(false, result);
            Assert.Equal(expectedvalue, storedPatient.Acronym);
        }

        [Fact]
        public async Task Delete_ReturnsTrue_IfPatientExists()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                ContentDir = "test"
            };
            var patientsList = new List<Patient>()
            {
                patient
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Delete(patient))
                .Callback<Patient>((p) => patientsList.Remove(patient));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.DeleteDirectory(It.IsAny<string>()));
            fileServiceMock.Setup(m => m.DeleteWebResource(It.IsAny<string>()));

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            Assert.Equal(true, result);
            Assert.Equal(0, patientsList.Count);
        }

        [Fact]
        public async Task Delete_ReturnsFalse_IfPatientNotExists()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                Acronym = "123"

            };
            var patientsList = new List<Patient>()
            {
                new Patient()
                {
                    ID = 2,
                    Acronym = "456"
                }
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Delete(patient))
                .Callback<Patient>((p) => patientsList.Remove(patient));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((Patient)null);
            var service = new PatientService(repoMock.Object, null);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            Assert.Equal(false, result);
            Assert.Equal(1, patientsList.Count);
        }

        [Fact]
        public async Task Delete_DeletesContentDir_IfPatientExists()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                ContentDir = "test"
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Delete(It.IsAny<Patient>()));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.DeleteDirectory(It.IsAny<string>()));
            fileServiceMock.Setup(m => m.DeleteWebResource(It.IsAny<string>()));

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            Assert.Equal(true, result);
            fileServiceMock.Verify(m => m.DeleteDirectory(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Delete_DeletesWebResource_IfPatientExists()
        {
            // set
            var patient = new Patient()
            {
                ID = 1,
                ContentDir = "test"
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Delete(It.IsAny<Patient>()));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.DeleteDirectory(It.IsAny<string>()));
            fileServiceMock.Setup(m => m.DeleteWebResource(It.IsAny<string>()));

            var service = new PatientService(repoMock.Object, fileServiceMock.Object);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            Assert.Equal(true, result);
            fileServiceMock.Verify(m => m.DeleteWebResource(It.IsAny<string>()), Times.Once());
        }
    }
}
