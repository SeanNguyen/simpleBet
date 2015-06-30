using SimpleBet.Models;
using SimpleBet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBet.Test.Api
{
    public class UserControllerTest
    {
        private IDataService dataService = new DataService();

        private User user1 { get { return new User() { Id = 1, FacebookId = 123, Name = "Jon Snow", AvatarUrl = "someUrl" }; } }
        private User userNoId { get { return new User() { FacebookId = 321, Name = "I am a robot", AvatarUrl = "someUrl" }; } }

        [Fact]
        public void TestGet()
        {
            User actual = this.dataService.GetUserById(1);
            User expected = this.user1;

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.FacebookId, actual.FacebookId);
            Assert.Equal(expected.AvatarUrl, actual.AvatarUrl);
        }

        [Fact]
        public void TestGetByFacebookId()
        {
            User actual = this.dataService.GetUserByFacebookId(123);
            User expected = this.user1;

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.FacebookId, actual.FacebookId);
            Assert.Equal(expected.AvatarUrl, actual.AvatarUrl);
        }

        [Fact]
        public void TestAdd()
        {
            User actual = this.dataService.AddUser(this.userNoId);
            User expected = this.userNoId;

            Assert.NotNull(actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.FacebookId, actual.FacebookId);
            Assert.Equal(expected.AvatarUrl, actual.AvatarUrl);
        }

        [Fact]
        public void TestUpdate()
        {
            User actual = this.user1;
            User expected = this.user1;
            expected.AvatarUrl = "changed avatar url";
            actual.AvatarUrl = "changed avatar url";
            this.dataService.UpdateUser(actual);
           

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.FacebookId, actual.FacebookId);
            Assert.Equal(expected.AvatarUrl, actual.AvatarUrl);
        }

    }
}
