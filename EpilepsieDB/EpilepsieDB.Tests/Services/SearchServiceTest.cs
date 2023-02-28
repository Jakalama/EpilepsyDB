using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Services;
using EpilepsieDB.Services.Impl;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Services
{
    public class SearchServiceTest : AbstractTest
    {
        public SearchServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var scanRepoMock = new Mock<IRepository<Scan>>();
            var annotationRepoMock = new Mock<IRepository<Annotation>>();

            // act
            var service = new SearchService(scanRepoMock.Object, annotationRepoMock.Object);
        }

        [Fact]
        public async Task EmptySearch_ReturnsNotNull()
        {
            // set
            var scans = new List<Scan>()
            {
            };
            var query = new SearchQuery();

            var scanRepoMock = new Mock<IRepository<Scan>>();
            scanRepoMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(scans);

            var annotationRepoMock = new Mock<IRepository<Annotation>>();

            var service = new SearchService(scanRepoMock.Object, annotationRepoMock.Object);

            // act
            IEnumerable<Scan> result = await service.Search(query);

            // assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EmptySearch_SearchResult_ContainsTwoPatients()
        {
            // set
            var scans = new List<Scan>()
            {
                new Scan()
                {
                    Recording = new Recording()
                    {
                        Patient = new Patient()
                        {
                            Acronym = "A",
                        },
                        RecordingNumber = "001"
                    },
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 1",
                    RecordInfo = "Record Info 1",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV"
                },
                new Scan()
                {
                    Recording = new Recording()
                    {
                        Patient = new Patient()
                        {
                            Acronym = "B",
                        },
                        RecordingNumber = "001"
                    },
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 2",
                    RecordInfo = "Record Info 2",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV"
                }
            };
            var query = new SearchQuery();

            var scanRepoMock = new Mock<IRepository<Scan>>();
            scanRepoMock.Setup(m => m.GetQueryable(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .Returns(scans.AsQueryable());

            var annotationRepoMock = new Mock<IRepository<Annotation>>();

            var service = new SearchService(scanRepoMock.Object, annotationRepoMock.Object);

            // act
            IEnumerable<Scan> result = await service.Search(query);

            // assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task EmptySearch_SearchResult_ContainsOneRecordingPerPatient()
        {
            // set
            var patient1 = new Patient()
            {
                Acronym = "A",
            };
            var patient2 = new Patient()
            {
                Acronym = "B",
            };
            var scans = new List<Scan>()
            {
                new Scan()
                {
                    Recording = new Recording()
                    {
                        Patient = patient1,
                        RecordingNumber = "001"
                    },
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 1",
                    RecordInfo = "Record Info 1",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV"
                },
                new Scan()
                {
                    Recording = new Recording()
                    {
                        Patient = patient2,
                        RecordingNumber = "001"
                    },
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 2",
                    RecordInfo = "Record Info 2",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV"
                }
            };
            var query = new SearchQuery();

            var scanRepoMock = new Mock<IRepository<Scan>>();
            scanRepoMock.Setup(m => m.GetQueryable(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .Returns(scans.AsQueryable());

            var annotationRepoMock = new Mock<IRepository<Annotation>>();

            var service = new SearchService(scanRepoMock.Object, annotationRepoMock.Object);

            // act
            IEnumerable<Scan> result = await service.Search(query);

            // assert
            Assert.Equal(2, result.Count());

            List<Scan> resultList = result.ToList();
            Assert.NotEqual(resultList[0].Recording.Patient, resultList[1].Recording.Patient);
        }

        [Fact]
        public async Task EmptySearch_SearchResult_ContainsOneScanPerRecording()
        {
            // set
            var patient1 = new Patient()
            {
                Acronym = "A",
            };
            var patient2 = new Patient()
            {
                Acronym = "B",
            };
            var recording1 = new Recording()
            {
                Patient = patient1,
                RecordingNumber = "001"
            };
            var recording2 = new Recording()
            {
                Patient = patient2,
                RecordingNumber = "001"
            };
            var scans = new List<Scan>()
            {
                new Scan()
                {
                    Recording = recording1,
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 1",
                    RecordInfo = "Record Info 1",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV"
                },
                new Scan()
                {
                    Recording = recording2,
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 2",
                    RecordInfo = "Record Info 2",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV"
                }
            };
            var query = new SearchQuery();

            var scanRepoMock = new Mock<IRepository<Scan>>();
            scanRepoMock.Setup(m => m.GetQueryable(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .Returns(scans.AsQueryable());

            var annoationRepoMock = new Mock<IRepository<Annotation>>();

            var service = new SearchService(scanRepoMock.Object, annoationRepoMock.Object);

            // act
            IEnumerable<Scan> result = await service.Search(query);


            // assert
            Assert.Equal(2, result.Count());

            List<Scan> resultList = result.ToList();
            Assert.NotEqual(resultList[0].Recording, resultList[1].Recording);
        }

        [Theory]
        [InlineData("16")]
        [InlineData("15")]
        [InlineData("14")]
        [InlineData("04")]
        [InlineData("05")]
        [InlineData("00")]
        [InlineData("16:15:14")]
        [InlineData("04:05:00")]
        public async Task TimeSearch_SearchResult_ContainsCorrect(string timeQuery)
        {
            // set
            var scans = new List<Scan>()
            {
                new Scan()
                {
                    ID = 1,
                    Recording = new Recording()
                    {
                        Patient = new Patient()
                        {
                            Acronym = "A",
                        },
                        RecordingNumber = "001"
                    },
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 1",
                    RecordInfo = "Record Info 1",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV",
                    StartDate = DateTime.Parse("10.11.2012 16:15:14"),
                    StartTime = DateTime.Parse("10.11.2012 16:15:14")
                },
                new Scan()
                {
                    ID = 2,
                    Recording = new Recording()
                    {
                        Patient = new Patient()
                        {
                            Acronym = "B",
                        },
                        RecordingNumber = "001"
                    },
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 2",
                    RecordInfo = "Record Info 2",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV",
                    StartDate = DateTime.Parse("01.02.1999 04:05:00"),
                    StartTime = DateTime.Parse("01.02.1999 04:05:00")
                }
            };
            var query = new SearchQuery()
            {
                Time = timeQuery
            };

            var scanRepoMock = new Mock<IRepository<Scan>>();
            scanRepoMock.Setup(m => m.GetQueryable(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .Returns(scans.AsQueryable());

            var annotationRepoMock = new Mock<IRepository<Annotation>>();

            var service = new SearchService(scanRepoMock.Object, annotationRepoMock.Object);

            // act
            IEnumerable<Scan> result = await service.Search(query);

            // assert
            Assert.Equal(1, result.Count());
            Assert.Contains(timeQuery, result.First().StartTime.ToString("HH:mm:ss"));
        }

        [Theory]
        [InlineData("10", 1)]
        [InlineData("11", 1)]
        [InlineData("12", 1)]
        [InlineData("2012", 1)]
        [InlineData("01", 2)]
        [InlineData("02", 1)]
        [InlineData("99", 1)]
        [InlineData("1999", 1)]
        [InlineData("10.11.2012", 1)]
        [InlineData("01.02.1999", 1)]
        public async Task DateSearch_SearchResult_ContainsCorrect(string dateQuery, int expectedNum)
        {
            // set
            var scans = new List<Scan>()
            {
                new Scan()
                {
                    ID = 1,
                    Recording = new Recording()
                    {
                        Patient = new Patient()
                        {
                            Acronym = "A",
                        },
                        RecordingNumber = "001"
                    },
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 1",
                    RecordInfo = "Record Info 1",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV",
                    StartDate = DateTime.Parse("10.11.2012 16:15:14"),
                    StartTime = DateTime.Parse("10.11.2012 16:15:14")
                },
                new Scan()
                {
                    ID = 2,
                    Recording = new Recording()
                    {
                        Patient = new Patient()
                        {
                            Acronym = "B",
                        },
                        RecordingNumber = "001"
                    },
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 2",
                    RecordInfo = "Record Info 2",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV",
                    StartDate = DateTime.Parse("01.02.1999 04:05:00"),
                    StartTime = DateTime.Parse("01.02.1999 04:05:00")
                }
            };
            var query = new SearchQuery()
            {
                Date = dateQuery
            };

            var scanRepoMock = new Mock<IRepository<Scan>>();
            scanRepoMock.Setup(m => m.GetQueryable(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .Returns(scans.AsQueryable());

            var annotationRepoMock = new Mock<IRepository<Annotation>>();

            var service = new SearchService(scanRepoMock.Object, annotationRepoMock.Object);

            // act
            IEnumerable<Scan> result = await service.Search(query);

            // assert
            Assert.Equal(expectedNum, result.Count());
            Assert.Contains(dateQuery, result.First().StartDate.ToString("dd.MM.yyyy"));
        }

        [Theory]
        [InlineData("A", 2)]
        [InlineData("B", 1)]
        [InlineData("C", 1)]
        [InlineData("Test", 2)]
        [InlineData("Test A", 2)]
        [InlineData("Test B", 1)]
        [InlineData("Test C", 1)]
        public async Task AnnotationSearch_SearchResult_ContainsCorrect(string annotationQuery, int expectedNum)
        {
            // set
            var scans = new List<Scan>()
            {
                new Scan()
                {
                    ID = 1,
                    Recording = new Recording()
                    {
                        Patient = new Patient()
                        {
                            Acronym = "A",
                        },
                        RecordingNumber = "001"
                    },
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 1",
                    RecordInfo = "Record Info 1",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV",
                },
                new Scan()
                {
                    ID = 2,
                    Recording = new Recording()
                    {
                        Patient = new Patient()
                        {
                            Acronym = "B",
                        },
                        RecordingNumber = "001"
                    },
                    ScanNumber = "1",
                    Version = "1",
                    PatientInfo = "Test Info 2",
                    RecordInfo = "Record Info 2",
                    Labels = "FA1",
                    TransducerTypes = "AlCg",
                    PhysicalDimensions = "mV",                
                }
            };
            var annotations = new List<Annotation>()
            {
                new Annotation()
                {
                    ScanID = 1,
                    Description = "Test A",
                },
                new Annotation()
                {
                    ScanID = 1,
                    Description = "Test B",
                },
                new Annotation()
                {
                    ScanID = 2,
                    Description = "Test A",
                },
                new Annotation()
                {
                    ScanID = 2,
                    Description = "Test C",
                },
                new Annotation()
                {
                    ScanID = 2,
                    Description = "Test C",
                }
            };

            var query = new SearchQuery()
            {
                Annot = annotationQuery
            };

            var scanRepoMock = new Mock<IRepository<Scan>>();
            scanRepoMock.Setup(m => m.GetQueryable(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .Returns(scans.AsQueryable());

            var annotationRepoMock = new Mock<IRepository<Annotation>>();
            annotationRepoMock.Setup(m => m.GetQueryable(
                It.IsAny<Expression<Func<Annotation, bool>>>(),
                It.IsAny<string>()))
                .Returns(annotations.AsQueryable());

            var service = new SearchService(scanRepoMock.Object, annotationRepoMock.Object);

            // act
            IEnumerable<Scan> result = await service.Search(query);

            // assert
            Assert.Equal(expectedNum, result.Count());
        }
    }
}
