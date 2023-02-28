using EpilepsieDB.Data;
using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Repositories.Impl;
using EpilepsieDB.Source.Wrapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Repositories
{
    public class ScanRepositoryTest : AbstractTest
    {
        public ScanRepositoryTest(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public void Constructor()
        {
            // set
            var contextMock = new Mock<IAppDbContext>();

            // act
            var repo = new PatientRepository(contextMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsScan()
        {
            // set
            var models = new List<Scan>()
            {
                new Scan(),
                new Scan()
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<Scan>>();

            contextMock.Setup(m => m.Set<Scan>())
                .Returns(setMock.Object);

            var repo = new ScanRepository(contextMock.Object);

            var wrapper = new Mock<IQueryableWrapper<Scan>>();
            wrapper.Setup(m => m.Where(
                It.IsAny<IQueryable<Scan>>(),
                It.IsAny<Expression<Func<Scan, bool>>>()))
                .Returns(models.AsQueryable());
            wrapper.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Scan>>()))
                .ReturnsAsync(models);

            FieldInfo field = typeof(ScanRepository).BaseType.GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(repo, wrapper.Object);

            // act
            var result = await repo.Get(4);

            // assert
            Assert.NotNull(result);
            Assert.Equal(models.First(), result);
            wrapper.Verify(m => m.Where(It.IsAny<IQueryable<Scan>>(), It.IsAny<Expression<Func<Scan, bool>>>()), Times.Once());
            wrapper.Verify(m => m.Include(It.IsAny<IQueryable<Scan>>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetAll_ReturnsScans()
        {
            // set
            var models = new List<Scan>()
            {
                new Scan(),
                new Scan()
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<Scan>>();

            contextMock.Setup(m => m.Set<Scan>())
                .Returns(setMock.Object);

            var repo = new ScanRepository(contextMock.Object);

            var wrapper = new Mock<IQueryableWrapper<Scan>>();
            wrapper.Setup(m => m.Where(
                It.IsAny<IQueryable<Scan>>(),
                It.IsAny<Expression<Func<Scan, bool>>>()))
                .Returns(models.AsQueryable());
            wrapper.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Scan>>()))
                .ReturnsAsync(models);

            FieldInfo field = typeof(ScanRepository).BaseType.GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(repo, wrapper.Object);

            // act
            var result = await repo.GetAll();

            // assert
            Assert.NotNull(result);
            Assert.Equal(models.Count(), result.Count());
            wrapper.Verify(m => m.Include(It.IsAny<IQueryable<Scan>>(), It.IsAny<string>()), Times.Once());
        }
    }
}
