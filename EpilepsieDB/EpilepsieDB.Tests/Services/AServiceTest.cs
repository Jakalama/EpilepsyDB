using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Services.Impl;
using EpilepsieDB.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace EpilepsieDB.Tests.Services
{
    public class AServiceTest : AbstractTest
    {
        public AServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Get_ReturnsModel()
        {
            // set
            var modelMock = new Mock<BaseModel>();
            modelMock.Object.ID = 1;

            var repoMock = new Mock<IRepository<BaseModel>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>())).
                ReturnsAsync(modelMock.Object);

            var service = new Mock<AService<BaseModel>>(repoMock.Object);

            // act
            var result = await service.Object.Get(It.IsAny<int>());

            Assert.Equal(modelMock.Object, result);
        }

        [Fact]
        public async Task Get_ReturnsListOfModels_Filtered()
        {
            // set
            var list = new List<BaseModel>();

            var repoMock = new Mock<IRepository<BaseModel>>();
            repoMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<BaseModel, bool>>>(),
                It.IsAny<string>())).
                ReturnsAsync(list);

            var service = new Mock<AService<BaseModel>>(repoMock.Object);
            service.CallBase = true;

            // act
            var result = await service.Object.Get();

            // assert
            repoMock.Verify(m => m.Get(
                It.IsAny<Expression<Func<BaseModel, bool>>>(),
                It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Get_ReturnsListOfModels_WithIncluded()
        {
            // set
            var list = new List<BaseModel>();

            var repoMock = new Mock<IRepository<BaseModel>>();
            repoMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<BaseModel, bool>>>(),
                It.IsAny<string>())).
                ReturnsAsync(list);

            var service = new Mock<AService<BaseModel>>(repoMock.Object);
            service.CallBase = true;

            // act
            var result = await service.Object.Get();

            // assert
            repoMock.Verify(m => m.Get(
                It.IsAny<Expression<Func<BaseModel, bool>>>(),
                It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetAll_ReturnsAllModels()
        {
            // set
            var modelMock1 = new Mock<BaseModel>();
            modelMock1.Object.ID = 1;
            var modelMock2 = new Mock<BaseModel>();
            modelMock2.Object.ID = 2;
            var list = new List<BaseModel>()
            {
                modelMock1.Object,
                modelMock2.Object
            };

            var repoMock = new Mock<IRepository<BaseModel>>();
            repoMock.Setup(m => m.GetAll())
                .ReturnsAsync(list);

            var service = new Mock<AService<BaseModel>>(repoMock.Object);

            // act
            var result = await service.Object.GetAll();

            // assert
            Assert.Equal(2, result.Count);
            Assert.Equal(list, result);
        }

        [Fact]
        public async Task Create_AddsModel()
        {
            // set
            var model = new Mock<BaseModel>();
            model.Object.ID = 1;

            var list = new List<BaseModel>();

            var repoMock = new Mock<IRepository<BaseModel>>();
            repoMock.Setup(m => m.Add(model.Object))
                .Callback<BaseModel>((p) => list.Add(p));

            var service = new Mock<AService<BaseModel>>(repoMock.Object);

            // act
            await service.Object.Create(model.Object);

            // assert
            Assert.Equal(1, list.Count);
            Assert.Equal(model.Object, list[0]);
        }

        [Fact]
        public async Task Update_ReturnsTrue_IfModelExists()
        {
            // set
            int updatedValue = 2;
            var model = new Mock<BaseModel>();
            model.Object.ID = 1;
            var updatedModel = new Mock<BaseModel>();
            updatedModel.Object.ID = updatedValue;

            var storedModel = model.Object;

            var repoMock = new Mock<IRepository<BaseModel>>();
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);
            repoMock.Setup(m => m.Update(updatedModel.Object))
                .Callback<BaseModel>((p) => storedModel = p);

            var service = new Mock<AService<BaseModel>>(repoMock.Object);

            // act
            var result = await service.Object.Update(updatedModel.Object);

            // assert
            Assert.Equal(true, result);
            Assert.Equal(updatedValue, storedModel.ID);
        }

        [Fact]
        public async Task Update_ReturnsFalse_IfModelNotExists()
        {
            // set
            int expectedValue = 1;
            var model = new Mock<BaseModel>();
            model.Object.ID = expectedValue;
            var updatedModel = new Mock<BaseModel>();
            updatedModel.Object.ID = 2;

            var storedModel = model.Object;

            var repoMock = new Mock<IRepository<BaseModel>>();
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(false);
            repoMock.Setup(m => m.Update(updatedModel.Object))
                .Callback<BaseModel>((p) => storedModel = p);

            var service = new Mock<AService<BaseModel>>(repoMock.Object);

            // act
            var result = await service.Object.Update(updatedModel.Object);

            // assert
            Assert.Equal(false, result);
            Assert.Equal(expectedValue, storedModel.ID);
        }

        [Fact]
        public async Task Delete_ReturnsTrue_IfModelExists()
        {
            // set
            var model = new Mock<BaseModel>();
            model.Object.ID = 1;
            var list = new List<BaseModel>()
            {
                model.Object
            };

            var repoMock = new Mock<IRepository<BaseModel>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(model.Object);
            repoMock.Setup(m => m.Delete(model.Object))
                .Callback<BaseModel>((p) => list.Remove(model.Object));
            
            var service = new Mock<AService<BaseModel>>(repoMock.Object);

            // act
            var result = await service.Object.Delete(It.IsAny<int>());

            // assert
            Assert.Equal(true, result);
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task Delete_ReturnsFalse_IfModelNotExists()
        {
            // set
            var model = new Mock<BaseModel>();
            model.Object.ID = 1;
            var list = new List<BaseModel>()
            {
                model.Object
            };

            var repoMock = new Mock<IRepository<BaseModel>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((BaseModel) null);
            repoMock.Setup(m => m.Delete(model.Object))
                .Callback<BaseModel>((p) => list.Remove(model.Object));

            var service = new Mock<AService<BaseModel>>(repoMock.Object);

            // act
            var result = await service.Object.Delete(It.IsAny<int>());

            // assert
            Assert.Equal(false, result);
            Assert.Equal(1, list.Count);
        }
    }
}
