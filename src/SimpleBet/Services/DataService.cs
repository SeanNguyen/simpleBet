using SimpleBet.Data;
using SimpleBet.Models;
using SimpleBet.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Services
{
    public class DataService : IDataService
    {
        private SimpleBetContext dbContext = new SimpleBetContext();

        public DataService() {
        }

        //Winning Item
        public IList<WinningItem> GetWinningItems()
        {
            return dbContext.WinningItems.ToList();
        }

        public IList<WinningItem> GetNonmonetaryItemsByCreator(int creatorId)
        {
            List<WinningItem> winningItems = this.dbContext.WinningItems.Where(w => w.CreatorId == creatorId && w.Type == WINNING_ITEM_TYPE.NONMONETARY).ToList();
            return winningItems;
        }

        public IList<WinningItem> GetWinningItemsByType(WINNING_ITEM_TYPE type)
        {
            List<WinningItem> winningItems = this.dbContext.WinningItems.Where(w => w.Type == type).ToList();
            return winningItems;
        }

        public IList<WinningItem> GetMonetaryItemsByCategory(WINNING_ITEM_CATEGORY category)
        {
            List<WinningItem> winningItems = this.dbContext.WinningItems.Where(w => w.Type == WINNING_ITEM_TYPE.MONETARY && w.Category == category).ToList();
            return winningItems;
        }

        public WinningItem GetWinningItem(int id)
        {
            return dbContext.WinningItems.Find(id);
        }

        public WinningItem AddWinningItem(WinningItem item)
        {
            dbContext.WinningItems.Add(item);
            dbContext.SaveChanges();
            return item;
        }

        public WinningItem UpdateWinningItem(WinningItem item)
        {
            dbContext.Entry(item).State = EntityState.Modified;
            this.dbContext.SaveChanges();
            return item;
        }

        public WinningItem RemoveWinningItem(int id)
        {
            WinningItem winningItem = dbContext.WinningItems.Find(id);
            this.dbContext.WinningItems.Remove(winningItem);
            this.dbContext.SaveChanges();
            return winningItem;
        }

        //User
        public IList<User> GetUsers()
        {
            return dbContext.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return dbContext.Users.Find(id);
        }

        public User GetUserByFacebookId(long facebookId)
        {
            User user = this.dbContext.Users.FirstOrDefault(u => u.FacebookId == facebookId);
            return user;
        }

        public User AddUser(User user)
        {
            //check if exists, then add
            User existingUser = this.dbContext.Users.Where(u => u.FacebookId == user.FacebookId).FirstOrDefault();
            if (existingUser == null)
            {
                this.dbContext.Users.Add(user);
                this.dbContext.SaveChanges();
                return user;
            }
            return existingUser;
        }

        public User UpdateUser(User user)
        {
            dbContext.Entry(user).State = EntityState.Modified;
            this.dbContext.SaveChanges();
            return user;
        }

        public User RemoveUser(int id)
        {
            User user = dbContext.Users.Find(id);
            this.dbContext.Users.Remove(user);
            this.dbContext.SaveChanges();
            return user;
        }

        //Bet
        public IList<Bet> GetBets()
        {
            return this.dbContext.Bets.ToList();
        }

        public Bet GetBet(int id)
        {
            Bet bet = this.dbContext.Bets.Where(b => b.Id == id)
                                        .Include(b => b.Participations.Select(p => p.User))
                                        .FirstOrDefault();
            if (bet != null)
            {
                updateBetState(bet);
            }
            return bet;
        }

        public Bet AddBet(Bet bet)
        {
            this.dbContext.Bets.Add(bet);
            this.dbContext.SaveChanges();
            return bet;
        }

        public Bet UpdateBet(Bet bet)
        {
            //check for action
            Bet existingBet = this.dbContext.Bets.Find(bet.Id);

            //add participants
            if(existingBet.State == BET_STATE.NONE && bet.State == BET_STATE.PENDING && existingBet.Participations.Count == 0)
            {
                existingBet.State = BET_STATE.PENDING;
                for (int i = 0; i < bet.Participations.Count; i++)
                {
                    BetUser betUser = bet.Participations.ElementAt(i);
                    dbContext.Entry(betUser).State = EntityState.Added;
                }
            }
            else if(existingBet.State == BET_STATE.ANSWERABLE && bet.State == BET_STATE.VERIFYING
                && getDisagreeParticipation(existingBet.Participations) == null)
            {
                existingBet.State = BET_STATE.VERIFYING;
                existingBet.WinningItemId = bet.WinningItemId;
                existingBet.WinningOption = bet.WinningOption;
                existingBet.WinningOptionChooser = bet.WinningOptionChooser;
                existingBet.VerifyStartTime = DateTime.UtcNow;
            }
            else if(existingBet.State == BET_STATE.VERIFYING && bet.State == BET_STATE.ANSWERABLE)
            {
                existingBet.State = BET_STATE.ANSWERABLE;
                existingBet.WinningItemId = bet.WinningItemId;
                existingBet.WinningOption = string.Empty;
                existingBet.WinningOptionChooser = -1;
                existingBet.AnswerStartTime = DateTime.UtcNow;
            }

            updateBetState(existingBet);
            resetParticipationState(existingBet);

            this.dbContext.SaveChanges();
            return bet;
        }

        public Bet RemoveBet(int id)
        {
            Bet bet = dbContext.Bets.Find(id);
            this.dbContext.Bets.Remove(bet);
            this.dbContext.SaveChanges();
            return bet;
        }

        //BetUser
        public BetUser GetBetUser(int betId, int userId)
        {
            BetUser betUser = this.dbContext.BetUsers.FirstOrDefault(bu => bu.BetId == betId && bu.UserId == userId);
            return betUser;
        }

        public BetUser UpdateBetUser(BetUser betUser)
        {
            dbContext.Entry(betUser).State = EntityState.Modified;
            this.dbContext.SaveChanges();

            //post-process bet
            //updateCancellingStatus(betUser.BetId);
            //updateFinallizableStatus(betUser.BetId);
            Bet bet = this.dbContext.Bets.Find(betUser.BetId);
            updateBetState(betUser.Bet);
            
            return betUser;
        }

        public BetUser RemoveBetUser(int betId, int userId)
        {
            BetUser betUser = this.dbContext.BetUsers.FirstOrDefault(bu => bu.BetId == betId && bu.UserId == userId);
            this.dbContext.BetUsers.Remove(betUser);
            this.dbContext.SaveChanges();
            return betUser;
        }
        
        //PRIVATE HELPER METHODS

        //private void updateCancellingStatus(int betId)
        //{
        //    Bet bet = this.dbContext.Bets.Find(betId);
        //    int agreeCount = 0;
        //    int disagreeCount = 0;
        //    for (int i = 0; i < bet.Participations.Count; i++)
        //    {
        //        BetUser participation = bet.Participations.ElementAt(i);
        //        if (participation.VoteCancelBetState == VoteCancelBetState.DISAGREE)
        //        {
        //            disagreeCount++;
        //        }
        //        else if (participation.VoteCancelBetState == VoteCancelBetState.AGREE
        //                || participation.VoteCancelBetState == VoteCancelBetState.CREATOR)
        //        {
        //            agreeCount++;
        //        }
        //    }
        //    if (agreeCount * 2 >= bet.Participations.Count)
        //    {
        //        bet.State = BET_STATE.CANCELLED;
        //    }
        //    else if (disagreeCount * 2 >= bet.Participations.Count)
        //    {
        //        bet.State = BET_STATE.CONFIRM;
        //        //reset all the edges
        //        for (int i = 0; i < bet.Participations.Count; i++)
        //        {
        //            BetUser participation = bet.Participations.ElementAt(i);
        //            participation.VoteCancelBetState = VoteCancelBetState.NONE;
        //        }
        //    }
        //    this.dbContext.SaveChanges();
        //}

        //private void updateFinallizableStatus(int betId)
        //{
        //    Bet bet = this.dbContext.Bets.Find(betId);
        //    bool allAnswered = true;
        //    for (int i = 0; i < bet.Participations.Count; i++)
        //    {
        //        BetUser participation = bet.Participations.ElementAt(i);
        //        if (participation.State < BetUserState.VOTED)
        //        {
        //            allAnswered = false;
        //            break;
        //        }
        //    }
        //    if (allAnswered)
        //    {
        //        bet.State = BET_STATE.FINALLIZABLE;
        //    }
        //    this.dbContext.SaveChanges();
        //}

        private Bet updateBetState(Bet bet)
        {
            switch(bet.State)
            {
                case BET_STATE.NONE:
                    break;
                case BET_STATE.PENDING:
                    updatePendingBetState(bet);
                    break;
                case BET_STATE.ANSWERABLE:
                    updateAnserableBetState(bet);
                    break;
                case BET_STATE.VERIFYING:
                    updateVerifyingBetState(bet);
                    break;
            }
            this.dbContext.SaveChanges();
            return bet;
        }

        private void updatePendingBetState(Bet bet)
        {
            bool isPendingTimeout = TimeUltility.isTimeout(bet.CreationTime, bet.PendingDuration);
            if (isPendingTimeout)
            {
                bet.State = BET_STATE.ANSWERABLE;
                bet.AnswerStartTime = bet.CreationTime.AddMinutes(bet.PendingDuration);
                setAllInactiveUserDecline(bet.Participations);
                updateBetState(bet);
            }
            else
            {
                bool areAllParticipantVoted = this.areAllParticipantVoted(bet.Participations);
                if (areAllParticipantVoted)
                {
                    bet.State = BET_STATE.ANSWERABLE;
                    bet.AnswerStartTime = DateTime.UtcNow;
                    setAllInactiveUserDecline(bet.Participations);
                }
            }
        }

        private void updateAnserableBetState(Bet bet)
        {
            DateTime answerStartTime = bet.AnswerStartTime ?? DateTime.UtcNow;
            bet.AnswerStartTime = answerStartTime;

            bool isTimeout = TimeUltility.isTimeout(answerStartTime, Bet.ANSWER_DURATION);
            if (isTimeout)
            {
                bet.State = BET_STATE.VERIFYING;
                bet.VerifyStartTime = answerStartTime.AddMinutes(Bet.ANSWER_DURATION);

                BetUser creator = getBetUserByUserId(bet.Participations, bet.CreatorId);
                if (!string.IsNullOrWhiteSpace(creator.Option))
                {
                    bet.WinningOption = creator.Option;
                }
                else
                {
                    bet.WinningOption = bet.Options.FirstOrDefault().Content;
                }
                updateBetState(bet);
            }

            //if time isn't up then check if this is a disagreed bet
            BetUser disagreeParticipation = getDisagreeParticipation(bet.Participations);
            if (disagreeParticipation != null)
            {
                foreach(BetUser betUser in bet.Participations)
                {
                    if(betUser.State != BETUSER_STATE.DECLINED && 
                        string.IsNullOrWhiteSpace(betUser.VotedAnswer) &&
                        !betUser.VoteDraw)
                        return;
                }
                //till here all participants have vote their answers
                bet.WinningOption = getWinningOptionWhenVoting(bet);
                bet.WinningOptionChooser = -1;
                bet.State = BET_STATE.FINALLIZED;
            }
        }

        private void updateVerifyingBetState(Bet bet)
        {
            DateTime verifyStartTime = bet.VerifyStartTime ?? DateTime.UtcNow;
            bet.VerifyStartTime = verifyStartTime;

            bool isTimeout = TimeUltility.isTimeout(verifyStartTime, Bet.VERIFY_DURATION);
            bool areAllParticipantAgree = this.areAllParticipantAgree(bet.Participations);

            if (isTimeout || areAllParticipantAgree)
            {
                bet.State = BET_STATE.FINALLIZED;
            }
        }

        private bool areAllParticipantVoted(ICollection<BetUser> participations)
        {
            foreach(BetUser betUser in participations)
            {
                if(betUser.State == BETUSER_STATE.PENDING || betUser.State == BETUSER_STATE.CONFIRMED)
                {
                    return false;
                }
            }
            return true;
        }

        private bool areAllParticipantAgree(ICollection<BetUser> participations)
        {
            foreach (BetUser betUser in participations)
            {
                if (betUser.State < BETUSER_STATE.AGREE && betUser.State != BETUSER_STATE.DECLINED)
                {
                    return false;
                }
            }
            return true;
        }

        private void setAllInactiveUserDecline(ICollection<BetUser> participations)
        {
            foreach(BetUser betUser in participations)
            {
                if(String.IsNullOrWhiteSpace(betUser.Option)) {
                    betUser.State = BETUSER_STATE.DECLINED;
                }
            }
        }

        private BetUser getBetUserByUserId(ICollection<BetUser> betUsers, int userId)
        {
            foreach(BetUser betUser in betUsers)
            {
                if(betUser.UserId == userId)
                {
                    return betUser;
                }
            }
            return null;
        }

        private void resetParticipationState(Bet bet)
        {
            ICollection<BetUser> activeBetUser = getActiveBetUser(bet.Participations);
            foreach (BetUser betUser in activeBetUser)
            {
                if(bet.State == BET_STATE.ANSWERABLE)
                { 
                    betUser.State = BETUSER_STATE.VOTED;
                }
            }
            this.dbContext.SaveChanges();
        }

        private ICollection<BetUser> getActiveBetUser(ICollection<BetUser> betUsers)
        {
            List<BetUser> activeBetUsers = new List<BetUser>();
            foreach (BetUser betUser in betUsers)
            {
                if(betUser.State != BETUSER_STATE.DECLINED)
                {
                    activeBetUsers.Add(betUser);
                }
            }
            return activeBetUsers;
        }

        private void ChangeBetState(Bet bet, BET_STATE state)
        {
            switch(state)
            {
                case BET_STATE.PENDING:
                    break;
                case BET_STATE.ANSWERABLE:
                    break;
                case BET_STATE.VERIFYING:
                    break;
            }
        }

        private BetUser getDisagreeParticipation(ICollection<BetUser> betUsers)
        {
            foreach(BetUser betUser in betUsers)
            {
                if (betUser.Disagree)
                    return betUser;
            }
            return null;
        }

        private string getWinningOptionWhenVoting(Bet bet)
        {
            //sort the option then conpare the top 2
            Dictionary<string, int> optionCountMaps = new Dictionary<string, int>();
            int drawCount = 0;

            foreach(BetUser betUser in bet.Participations)
            {
                if (betUser.VoteDraw)
                {
                    drawCount++;
                }
                else if(!string.IsNullOrWhiteSpace(betUser.VotedAnswer))
                {
                    if(!optionCountMaps.ContainsKey(betUser.VotedAnswer))
                    {
                        optionCountMaps.Add(betUser.VotedAnswer, 0);
                    }
                    optionCountMaps[betUser.VotedAnswer]++;
                }
            }

            var sortedOptions = from pair in optionCountMaps
                                orderby pair.Value descending
                                select pair;

            string topOption = string.Empty;
            string secondTopOption = string.Empty;
            if (sortedOptions.Count() >= 1)
            {
                topOption = sortedOptions.ElementAt(0).Key;
            }
            if(sortedOptions.Count() >= 2)
            {
                secondTopOption = sortedOptions.ElementAt(1).Key;
                if (optionCountMaps[topOption] == optionCountMaps[secondTopOption])
                    return string.Empty;
            }

            if(!string.IsNullOrWhiteSpace(topOption) && drawCount < optionCountMaps[topOption])
            {
                return topOption;
            }

            //empty option means draw / no option win
            return string.Empty;
        }
    }
}
