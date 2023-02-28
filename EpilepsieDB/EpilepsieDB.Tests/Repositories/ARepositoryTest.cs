using EpilepsieDB.Data;
using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Repositories.Impl;
using EpilepsieDB.Source.Wrapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Repositories
{
    public class ARepositoryTest : AbstractTest
    {
        public ARepositoryTest(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public async Task Get_ReturnsModel()
        {
            // set
            var model = new Patient()
            {
                ID = 1
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<BaseModel>>();

            contextMock.Setup(m => m.Set<BaseModel>()).Returns(setMock.Object);
            setMock.Setup(m => m.FindAsync(It.IsAny<int>())).ReturnsAsync(model);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            // act
            var result = await repo.Object.Get(It.IsAny<int>());


            // assert
            Assert.NotNull(result);
            Assert.Equal(model.ID, result.ID);
        }

        [Fact]
        public async Task Get_ReturnsNull_IfIDIsInvalid()
        {
            // set
            BaseModel model = null;

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<BaseModel>>();

            contextMock.Setup(m => m.Set<BaseModel>()).Returns(setMock.Object);
            setMock.Setup(m => m.FindAsync(It.IsAny<int>())).ReturnsAsync(model);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            // act
            var result = await repo.Object.Get(It.IsAny<int>());

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetNoTracking()
        {
            // set
            var models = new List<BaseModel>()
            {
                new Patient()
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<BaseModel>>();

            contextMock.Setup(m => m.Set<BaseModel>())
                .Returns(setMock.Object);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            var wrapper = new Mock<IQueryableWrapper<BaseModel>>();
            wrapper.Setup(m => m.Where(
                It.IsAny<IQueryable<BaseModel>>(),
                It.IsAny<Expression<Func<BaseModel, bool>>>()))
                .Returns(models.AsQueryable());
            wrapper.Setup(m => m.AsNoTracking(
                It.IsAny<IQueryable<BaseModel>>()));
            wrapper.Setup(m => m.FirstOrDefaultAsync(
                It.IsAny<IQueryable<BaseModel>>()))
                .ReturnsAsync(models.First());

            FieldInfo field = typeof(ARepository<BaseModel>).GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(repo.Object, wrapper.Object);

            // act
            var result = await repo.Object.GetNoTracking(It.IsAny<int>());

            // assert
            Assert.NotNull(result);
            wrapper.Verify(m => m.Where(It.IsAny<IQueryable<BaseModel>>(), It.IsAny<Expression<Func<BaseModel, bool>>>()), Times.Once());
            wrapper.Verify(m => m.AsNoTracking(It.IsAny<IQueryable<BaseModel>>()), Times.Once());
            wrapper.Verify(m => m.FirstOrDefaultAsync(It.IsAny<IQueryable<BaseModel>>()), Times.Once());
        }

        [Fact]
        public async Task Get_ReturnsListOfModels_Filtered()
        {
            // set
            var models = new List<BaseModel>()
            {
                (new Mock<BaseModel>()).Object,
                (new Mock<BaseModel>()).Object,
                (new Mock<BaseModel>()).Object,
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<BaseModel>>();

            contextMock.Setup(m => m.Set<BaseModel>())
                .Returns(setMock.Object);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            var wrapper = new Mock<IQueryableWrapper<BaseModel>>();
            wrapper.Setup(m => m.Where(
                It.IsAny<IQueryable<BaseModel>>(),
                It.IsAny<Expression<Func<BaseModel, bool>>>()))
                .Returns(models.AsQueryable());
            wrapper.Setup(m => m.ToListAsync(It.IsAny<IQueryable<BaseModel>>()))
                .ReturnsAsync(models);

            FieldInfo field = typeof(ARepository<BaseModel>).GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(repo.Object, wrapper.Object);

            // act
            var result = await repo.Object.Get(filter: p => p.ID != 4);

            // assert
            Assert.NotNull(result);
            Assert.Equal(models.Count(), result.Count());
            wrapper.Verify(m => m.Where(It.IsAny<IQueryable<BaseModel>>(), It.IsAny<Expression<Func<BaseModel, bool>>>()), Times.Once());
        }

        [Fact]
        public async Task Get_ReturnsListOfPatients_WithIncluded()
        {
            // set
            List<Recording> recordings = new List<Recording>()
            {
                new Recording(),
                new Recording()
            };
            var models = new List<BaseModel>()
            {
                new Patient()
                {
                    Recordings = recordings
                }
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<BaseModel>>();

            contextMock.Setup(m => m.Set<BaseModel>())
                .Returns(setMock.Object);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            var wrapper = new Mock<IQueryableWrapper<BaseModel>>();
            wrapper.Setup(m => m.Include(
                It.IsAny<IQueryable<BaseModel>>(),
                It.IsAny<string>()))
                .Returns(models.AsQueryable());
            wrapper.Setup(m => m.ToListAsync(It.IsAny<IQueryable<BaseModel>>()))
                .ReturnsAsync(models);

            FieldInfo field = typeof(ARepository<BaseModel>).GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(repo.Object, wrapper.Object);

            // act
            var result = await repo.Object.Get(includeProperties: "Recordings");

            // assert
            Assert.NotNull(result);
            Assert.Equal(models.Count(), result.Count());
            wrapper.Verify(m => m.Include(It.IsAny<IQueryable<BaseModel>>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void Get_ReturnsQueryableOfPatients_Filtered()
        {
            // set
            var models = new List<BaseModel>()
            {
                (new Mock<BaseModel>()).Object,
                (new Mock<BaseModel>()).Object,
                (new Mock<BaseModel>()).Object,
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<BaseModel>>();

            contextMock.Setup(m => m.Set<BaseModel>())
                .Returns(setMock.Object);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            var wrapper = new Mock<IQueryableWrapper<BaseModel>>();
            wrapper.Setup(m => m.Include(
                It.IsAny<IQueryable<BaseModel>>(),
                It.IsAny<string>()))
                .Returns(models.AsQueryable());

            FieldInfo field = typeof(ARepository<BaseModel>).GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(repo.Object, wrapper.Object);

            // act
            var result = repo.Object.GetQueryable(includeProperties: "Recordings");

            // assert
            Assert.NotNull(result);
            Assert.Equal(models.Count(), result.Count());
            wrapper.Verify(m => m.Include(It.IsAny<IQueryable<BaseModel>>(),
                It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void Get_ReturnsQueryableOfPatients_WithIncluded()
        {
            // set
            List<Recording> recordings = new List<Recording>()
            {
                new Recording(),
                new Recording()
            };
            var models = new List<BaseModel>()
            {
                new Patient()
                {
                    Recordings = recordings
                }
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<BaseModel>>();

            contextMock.Setup(m => m.Set<BaseModel>())
                .Returns(setMock.Object);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            var wrapper = new Mock<IQueryableWrapper<BaseModel>>();
            wrapper.Setup(m => m.Include(
                It.IsAny<IQueryable<BaseModel>>(),
                It.IsAny<string>()))
                .Returns(models.AsQueryable());

            FieldInfo field = typeof(ARepository<BaseModel>).GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(repo.Object, wrapper.Object);

            // act
            var result = repo.Object.GetQueryable(includeProperties: "Recordings");

            // assert
            Assert.NotNull(result);
            Assert.Equal(models.Count(), result.Count());
            wrapper.Verify(m => m.Include(It.IsAny<IQueryable<BaseModel>>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Get_ReturnsListOfModels_IfDbSetContainsElements()
        {
            // set
            var models = new List<BaseModel>()
            {
                new Patient()
                {
                    ID = 1
                },
                new Patient()
                {
                    ID = 2
                },
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = models.AsQueryable().BuildMockDbSet();

            contextMock.Setup(m => m.Set<BaseModel>()).Returns(setMock.Object);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            // act
            var result = await repo.Object.GetAll();

            // assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Get_ReturnsEmptyList_IfDbSetIsEmpty()
        {
            // set
            var models = new List<BaseModel>();

            var contextMock = new Mock<IAppDbContext>();
            var setMock = models.AsQueryable().BuildMockDbSet();

            contextMock.Setup(m => m.Set<BaseModel>()).Returns(setMock.Object);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            // act
            var result = await repo.Object.GetAll();

            // assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public async Task Exists_ReturnsTrue_IfModelWithIDExists()
        {
            // set
            var models = new List<BaseModel>
            {
                new Patient()
                {
                    ID = 1
                }
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = models.AsQueryable().BuildMockDbSet();

            contextMock.Setup(m => m.Set<BaseModel>()).Returns(setMock.Object);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            // act
            var result = await repo.Object.Exists(1);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task Exists_ReturnsTrue_IfPatientWithIDNotExists()
        {
            // set
            var models = new List<BaseModel>
            {
                new Patient()
                {
                    ID = 1
                }
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = models.AsQueryable().BuildMockDbSet();

            contextMock.Setup(m => m.Set<BaseModel>()).Returns(setMock.Object);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            // act
            var result = await repo.Object.Exists(2);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task Add_AddsModel()
        {
            // set
            var model = new Patient()
            {
                ID = 1
            };
            var models = new List<BaseModel>();

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<BaseModel>>();

            contextMock.Setup(m => m.Set<BaseModel>()).Returns(setMock.Object);
            setMock.Setup(m => m.Add(It.IsAny<BaseModel>())).Callback<BaseModel>((p) => models.Add(p));

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            // act
            await repo.Object.Add(model);

            // assert
            Assert.True(models.Any());
            Assert.Equal(models.First().ID, model.ID);
            contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Update_UpdatesPatient()
        {
            // Note:
            // As EF Core generates optimized SQL queries to update
            // the database entry, we skip testing the behaviour trying to
            // update data values which are not provided in the updated data model

            // set
            var model = new Patient()
            {
                ID = 1,
                Acronym = "123"
            };
            BaseModel storedModel = new Patient()
            {
                ID = 1
            };

            var list = new List<BaseModel>() { storedModel };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = list.AsQueryable().BuildMockDbSet();

            contextMock.Setup(m => m.Set<BaseModel>()).Returns(setMock.Object);
            setMock.Setup(m => m.Update(It.IsAny<BaseModel>())).Callback<BaseModel>((p) => storedModel = p);

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            // act
            await repo.Object.Update(model);

            // assert
            Assert.Equal(model.ID, storedModel.ID);
            Assert.Equal(model.Acronym, ((Patient)storedModel).Acronym);
            contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Delete_DeletesPatient()
        {
            // set
            var model = new Patient()
            {
                ID = 1
            };
            var models = new List<BaseModel> { model };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<BaseModel>>();

            contextMock.Setup(m => m.Set<BaseModel>()).Returns(setMock.Object);
            setMock.Setup(m => m.Remove(It.IsAny<BaseModel>())).Callback<BaseModel>((p) => models.Remove(p));

            var repo = new Mock<ARepository<BaseModel>>(contextMock.Object);
            repo.CallBase = true;

            // act
            await repo.Object.Delete(model);

            // assert
            Assert.False(models.Any());
            contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }
    }
}
