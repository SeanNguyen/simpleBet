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
                participations.Add(new BetUser() { State = BETUSER_STATE.VOTED, UserId = 1, Option = "This is option a" });
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
                    CreationTime = DateTime.UtcNow,
                    PendingDuration = 60 * 24,
                    CreatorId = 1,
                    Question = "This is a question",
                    State = BET_STATE.NONE,
                    Options = options,
                    WinningItemId = 1
                };
                return bet;
            }
        }

        private Bet betStatePending
        {
            get
            {
                Bet bet = new Bet()
                {
                    BetType = BET_TYPE.ONE_MANY,
                    CreationTime = DateTime.UtcNow,
                    PendingDuration = 60 * 24,
                    CreatorId = 1,
                    Question = "This is a question",
                    State = BET_STATE.PENDING,
                    Options = options,
                    Participations = betUsers,
                    WinningItemId = 1
                };
                return bet;
            }
        }

        private Bet betStateAnswerable
        {
            get
            {
                Bet bet = new Bet()
                {
                    BetType = BET_TYPE.ONE_MANY,
                    CreationTime = DateTime.UtcNow,
                    PendingDuration = 60 * 24,
                    CreatorId = 1,
                    Question = "This is a question",
                    State = BET_STATE.ANSWERABLE,
                    Options = options,
                    Participations = betUsers,
                    WinningItemId = 1
                };
                return bet;
            }
        }

        private Bet betStateVerifying
        {
            get
            {
                Bet bet = new Bet()
                {
                    BetType = BET_TYPE.ONE_MANY,
                    CreationTime = DateTime.UtcNow,
                    PendingDuration = 60 * 24,
                    CreatorId = 1,
                    Question = "This is a question",
                    State = BET_STATE.VERIFYING,
                    Options = options,
                    Participations = betUsers,
                    WinningItemId = 1
                };
                return bet;
            }
        }

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
            Bet bet = this.betStatePending;
            bet.CreationTime = bet.CreationTime.AddMinutes(-bet.PendingDuration - 1);
            bet = this.dataService.AddBet(bet);
            bet = this.dataService.GetBet(bet.Id);

            Assert.Equal(BET_STATE.ANSWERABLE, bet.State);
        }

        [Fact]
        public void pendingTimeoutThenAnswerTimeout()
        {
            Bet bet = this.betStatePending;
            bet.CreationTime = DateTime.UtcNow.AddMinutes(-bet.PendingDuration - Bet.ANSWER_DURATION - 1);
            bet.Participations.ElementAt(1).State = BETUSER_STATE.VOTED;
            bet.Participations.ElementAt(1).Option = "This is option a";
            bet = this.dataService.AddBet(bet);
            bet = this.dataService.GetBet(bet.Id);

            Assert.Equal(BET_STATE.VERIFYING, bet.State);
        }

        [Fact]
        public void pendingTimeoutThenVerifyTimeout()
        {
            Bet bet = this.betStatePending;
            bet = this.dataService.AddBet(bet);
            bet.CreationTime = DateTime.UtcNow.AddMinutes(- bet.PendingDuration 
                - Bet.ANSWER_DURATION - Bet.VERIFY_DURATION - 1);
            bet = this.dataService.GetBet(bet.Id);

            Assert.Equal(BET_STATE.FINALLIZED, bet.State);
        }

        [Fact]
        public void answerTimeout()
        {
            Bet bet = this.betStateAnswerable;
            bet = this.dataService.AddBet(bet);
            bet.CreationTime = DateTime.UtcNow.AddMinutes(-bet.PendingDuration
                - Bet.ANSWER_DURATION - 1);
            bet.AnswerStartTime = DateTime.UtcNow.AddMinutes(-Bet.ANSWER_DURATION - 1);
            bet = this.dataService.GetBet(bet.Id);

            Assert.Equal(BET_STATE.VERIFYING, bet.State);
        }

        [Fact]
        public void answerTimeoutThenVerifyTimeout()
        {
            Bet bet = this.betStateAnswerable;
            bet = this.dataService.AddBet(bet);
            bet.CreationTime = DateTime.UtcNow.AddMinutes(-bet.PendingDuration
                - Bet.ANSWER_DURATION - Bet.VERIFY_DURATION - 1);
            bet.AnswerStartTime = DateTime.UtcNow.AddMinutes(-Bet.ANSWER_DURATION - Bet.VERIFY_DURATION - 1);
            bet = this.dataService.GetBet(bet.Id);

            Assert.Equal(BET_STATE.FINALLIZED, bet.State);
        }

        [Fact]
        public void verifyTimeout()
        {
            Bet bet = this.betStateVerifying;
            bet = this.dataService.AddBet(bet);
            bet.CreationTime = DateTime.UtcNow.AddMinutes(-bet.PendingDuration
                - Bet.ANSWER_DURATION - Bet.VERIFY_DURATION - 1);
            bet.AnswerStartTime = DateTime.UtcNow.AddMinutes(-Bet.ANSWER_DURATION - Bet.VERIFY_DURATION - 1);
            bet.VerifyStartTime = DateTime.UtcNow.AddMinutes(-Bet.VERIFY_DURATION - 1);
            bet = this.dataService.GetBet(bet.Id);

            Assert.Equal(BET_STATE.FINALLIZED, bet.State);
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
