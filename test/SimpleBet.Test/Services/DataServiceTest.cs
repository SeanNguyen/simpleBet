using SimpleBet.Models;
using SimpleBet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBet.Test.Services
{
    public class DataServiceTest
    {
        private IDataService dataService = new DataService();

        private ICollection<Option> options
        {
            get
            {
                List<Option> options = new List<Option>();
                options.Add(new Option() { Content = "This is option a" });
                options.Add(new Option() { Content = "This is option b" });
                return options;
            }
        }

        private ICollection<BetUser> betUsers {
            get
            {
                List<BetUser> participations = new List<BetUser>();
                participations.Add(new BetUser() { State = BETUSER_STATE.CONFIRMED, UserId = 1 });
                participations.Add(new BetUser() { State = BETUSER_STATE.PENDING, UserId = 2 });
                return participations;
            }
        }

        private Bet betStateNone {
            get
            {
                Bet bet = new Bet()
                {
                    BetType = BET_TYPE.ONE_MANY,
                    CreationTime = DateTime.Now,
                    PendingDuration = 1000,
                    CreatorId = 1,
                    Question = "This is a question",
                    State = BET_STATE.NONE,
                    Options = options,
                    WinningItemId = 1
                };
                return bet;
            } }

        [Fact]
        public void createBet()
        {
            Bet actual = this.betStateNone;
            actual = this.dataService.AddBet(this.betStateNone);
            actual.Participations = this.betUsers;
            actual.State = BET_STATE.PENDING;
            actual = this.dataService.UpdateBet(actual);

            Assert.NotNull(actual.Id);
            Bet expected = this.betStateNone;
            expected.Id = actual.Id;
            expected.Participations = this.betUsers;
            expected.State = BET_STATE.PENDING;
            expected.CreationTime = actual.CreationTime;

            Assert.Equal(true, compareBets(expected, actual));
        }

        [Fact]
        public void pendingTimeout()
        {
            Bet bet = this.dataService.GetBet(1);
        }

        //private helper methods
        private bool compareBets(Bet bet1, Bet bet2)
        {
            if (!(bet1.Id == bet2.Id &&
                bet1.State == bet2.State &&
                bet1.Question.Equals(bet2.Question) &&
                bet1.WinningItemId == bet2.WinningItemId &&
                bet1.CreatorId == bet2.CreatorId &&
                bet1.CreationTime.Equals(bet2.CreationTime) &&
                bet1.PendingDuration == bet2.PendingDuration))
            {
                return false;
            }
            if(!(bet1.WinningOption == null && bet2.WinningOption == null) && !bet1.WinningOption.Equals(bet2.WinningOption))
            {
                return false;
            }
            if (!(bet1.WinningOptionChooser == null && bet2.WinningOptionChooser == null) && !bet1.WinningOptionChooser.Equals(bet2.WinningOptionChooser))
            {
                return false;
            }
            if (!(bet1.AnswerStartTime == null && bet2.AnswerStartTime == null) && !bet1.AnswerStartTime.Equals(bet2.AnswerStartTime))
            {
                return false;
            }
            if (!(bet1.VerifyStartTime == null && bet2.VerifyStartTime == null) && !bet1.VerifyStartTime.Equals(bet2.VerifyStartTime))
            {
                return false;
            }
            return true;
        }
    }
}
