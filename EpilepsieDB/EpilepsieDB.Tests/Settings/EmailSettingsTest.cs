using EpilepsieDB.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Settings
{
    public class EmailSettingsTest : AbstractTest
    {
        public EmailSettingsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Disabled_ReturnsCorrect()
        {
            // set
            var expected = true;
            var settings = new EmailSettings();
            settings.Disabled = expected;

            // act
            var result = settings.Disabled;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MailServer_ReturnsCorrect()
        {
            // set
            var expected = "test";
            var settings = new EmailSettings();
            settings.MailServer = expected;

            // act
            var result = settings.MailServer;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MailPort_ReturnsCorrect()
        {
            // set
            var expected = 543;
            var settings = new EmailSettings();
            settings.MailPort = expected;

            // act
            var result = settings.MailPort;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SenderEmail_ReturnsCorrect()
        {
            // set
            var expected = "test";
            var settings = new EmailSettings();
            settings.SenderEmail = expected;

            // act
            var result = settings.SenderEmail;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SenderName_ReturnsCorrect()
        {
            // set
            var expected = "test";
            var settings = new EmailSettings();
            settings.SenderName = expected;

            // act
            var result = settings.SenderName;

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Password_ReturnsCorrect()
        {
            // set
            var expected = "test";
            var settings = new EmailSettings();
            settings.Password = expected;

            // act
            var result = settings.Password;

            // assert
            Assert.Equal(expected, result);
        }
    }
}
