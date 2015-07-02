using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SimpleBet.Controllers;
using SimpleBet.Models;
using SimpleBet.Services;

namespace SimpleBet.Test.Api
{
    public class BetControllerTest
    {
        private IDataService dataService = new DataService();
        private User user1 { get { return new User() { Id = 1, FacebookId = 123, Name = "Jon Snow", AvatarUrl = "someUrl" }; } }

        Bet betNoId;

        public BetControllerTest()
        {
            List<Option> options = new List<Option>();
            options.Add(new Option() { Content = "This is option a" });
            options.Add(new Option() { Content = "This is option b" });

            List<BetUser> participations = new List<BetUser>();
            participations.Add(new BetUser() { State = BETUSER_STATE.CONFIRMED, UserId = 1 });
            participations.Add(new BetUser() { State = BETUSER_STATE.PENDING, UserId = 2 });

            this.betNoId = new Bet()
            {
                BetType = BET_TYPE.ONE_MANY,
                CreationTime = DateTime.Now,
                PendingDuration = 1000,
                CreatorId = 1,
                Question = "This is a question",
                State = BET_STATE.ANSWERABLE,
                Options = options,
                Participations = participations
            };
        }

        [Fact]
        public void TestAddBet()
        {
        }
    }
}
