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
    public class InviteExtensionTest : AbstractTest
    {
        public InviteExtensionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData("test")]
        [InlineData("email")]
        public void ToUserInvite_SetsEmail(string value)
        {
            // set
            var invite = new Invite();
            invite.Email = value;

            // act
            var result = invite.ToUserInvite();

            // assert
            Assert.Equal(value, result.Email);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToUserInvite_SetsIsSystemadmin(bool value)
        {
            // set
            var invite = new Invite();
            invite.IsSystemadmin = value;

            // act
            var result = invite.ToUserInvite();

            // assert
            Assert.Equal(value, result.IsSystemadmin);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToUserInvite_SetsIsUser(bool value)
        {
            // set
            var invite = new Invite();
            invite.IsUser = value;

            // act
            var result = invite.ToUserInvite();

            // assert
            Assert.Equal(value, result.IsUser);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToUserInvite_SetsIsScanCreator(bool value)
        {
            // set
            var invite = new Invite();
            invite.IsScanCreator = value;

            // act
            var result = invite.ToUserInvite();

            // assert
            Assert.Equal(value, result.IsScanCreator);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToUserInvite_SetsIsScanDownloader(bool value)
        {
            // set
            var invite = new Invite();
            invite.IsScanDownloader = value;

            // act
            var result = invite.ToUserInvite();

            // assert
            Assert.Equal(value, result.IsScanDownloader);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToUserInvite_SetsIsScanReader(bool value)
        {
            // set
            var invite = new Invite();
            invite.IsScanReader = value;

            // act
            var result = invite.ToUserInvite();

            // assert
            Assert.Equal(value, result.IsScanReader);
        }
    }
}
