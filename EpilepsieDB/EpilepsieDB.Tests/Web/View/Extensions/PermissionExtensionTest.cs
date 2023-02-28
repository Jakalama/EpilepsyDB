using EpilepsieDB.Services;
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
    public class PermissionExtensionTest : AbstractTest
    {
        public PermissionExtensionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsSystemadmin_IsSetCorrectly(bool value)
        {
            // set
            Permissions permissions = new Permissions();
            permissions.IsSystemadmin = value;

            // act
            UserPermissions model = permissions.ToUserPermissions();

            // assert
            Assert.Equal(value, model.IsSystemadmin);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsUser_IsSetCorrectly(bool value)
        {
            // set
            Permissions permissions = new Permissions();
            permissions.IsUser = value;

            // act
            UserPermissions model = permissions.ToUserPermissions();

            // assert
            Assert.Equal(value, model.IsUser);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsScanCreator_IsSetCorrectly(bool value)
        {
            // set
            Permissions permissions = new Permissions();
            permissions.IsScanCreator = value;

            // act
            UserPermissions model = permissions.ToUserPermissions();

            // assert
            Assert.Equal(value, model.IsScanCreator);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsScanDownloader_IsSetCorrectly(bool value)
        {
            // set
            Permissions permissions = new Permissions();
            permissions.IsScanDownloader = value;

            // act
            UserPermissions model = permissions.ToUserPermissions();

            // assert
            Assert.Equal(value, model.IsScanDownloader);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsScanReader_IsSetCorrectly(bool value)
        {
            // set
            Permissions permissions = new Permissions();
            permissions.IsScanReader = value;

            // act
            UserPermissions model = permissions.ToUserPermissions();

            // assert
            Assert.Equal(value, model.IsScanReader);
        }
    }
}
