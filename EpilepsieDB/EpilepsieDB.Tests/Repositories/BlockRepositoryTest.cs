using EpilepsieDB.Data;
using EpilepsieDB.Models;
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
    public class BlockRepositoryTest : AbstractTest
    {
        public BlockRepositoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var contextMock = new Mock<IAppDbContext>();

            // act
            var repo = new BlockRepository(contextMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsBlock()
        {
            // set
            var models = new List<Block>()
            {
                new Block(),
                new Block()
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<Block>>();

            contextMock.Setup(m => m.Set<Block>())
                .Returns(setMock.Object);

            var repo = new BlockRepository(contextMock.Object);

            var wrapper = new Mock<IQueryableWrapper<Block>>();
            wrapper.Setup(m => m.Where(
                It.IsAny<IQueryable<Block>>(),
                It.IsAny<Expression<Func<Block, bool>>>()))
                .Returns(models.AsQueryable());
            wrapper.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Block>>()))
                .ReturnsAsync(models);

            FieldInfo field = typeof(BlockRepository).BaseType.GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(repo, wrapper.Object);

            // act
            var result = await repo.Get(4);

            // assert
            Assert.NotNull(result);
            Assert.Equal(models.First(), result);
            wrapper.Verify(m => m.Where(It.IsAny<IQueryable<Block>>(), It.IsAny<Expression<Func<Block, bool>>>()), Times.Once());
            wrapper.Verify(m => m.Include(It.IsAny<IQueryable<Block>>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetAll_ReturnsBlocks()
        {
            // set
            var models = new List<Block>()
            {
                new Block(),
                new Block()
            };

            var contextMock = new Mock<IAppDbContext>();
            var setMock = new Mock<DbSet<Block>>();

            contextMock.Setup(m => m.Set<Block>())
                .Returns(setMock.Object);

            var repo = new BlockRepository(contextMock.Object);

            var wrapper = new Mock<IQueryableWrapper<Block>>();
            wrapper.Setup(m => m.Where(
                It.IsAny<IQueryable<Block>>(),
                It.IsAny<Expression<Func<Block, bool>>>()))
                .Returns(models.AsQueryable());
            wrapper.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Block>>()))
                .ReturnsAsync(models);

            FieldInfo field = typeof(BlockRepository).BaseType.GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(repo, wrapper.Object);

            // act
            var result = await repo.GetAll();

            // assert
            Assert.NotNull(result);
            Assert.Equal(models.Count(), result.Count());
            wrapper.Verify(m => m.Include(It.IsAny<IQueryable<Block>>(), It.IsAny<string>()), Times.Once());
        }
    }
}
