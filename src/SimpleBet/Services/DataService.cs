﻿using SimpleBet.Data;
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
        private SimpleBetContext dbContext;

        public DataService(SimpleBetContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Winning Item
        public IList<WinningItem> GetWinningItems()
        {
            return dbContext.WinningItems.ToList();
        }

        public IList<WinningItem> GetWinningItemsByCreator(int creatorId)
        {
            List<WinningItem> winningItems = this.dbContext.WinningItems.Where(w => w.CreatorId == creatorId).ToList();
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
            updateBetState(bet);
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
                for (int i = 0; i < bet.Participations.Count; i++)
                {
                    BetUser betUser = bet.Participations.ElementAt(i);
                    dbContext.Entry(betUser).State = EntityState.Added;
                }
            }
            else if(existingBet.State == BET_STATE.ANSWERABLE && bet.State == BET_STATE.VERIFYING)
            {
                existingBet.State = BET_STATE.VERIFYING;
                existingBet.WinningItemId = bet.WinningItemId;
                existingBet.WinningOption = bet.WinningOption;
                existingBet.WinningOptionChooser = bet.WinningOptionChooser;
                existingBet.VerifyStartTime = DateTime.Now;
            }
            else if(existingBet.State == BET_STATE.VERIFYING && bet.State == BET_STATE.ANSWERABLE)
            {
                existingBet.State = BET_STATE.ANSWERABLE;
                existingBet.WinningItemId = bet.WinningItemId;
                existingBet.WinningOption = bet.WinningOption;
                existingBet.WinningOptionChooser = bet.WinningOptionChooser;
                existingBet.AnswerStartTime = DateTime.Now;
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
            if(bet.State == BET_STATE.NONE)
            {
                return bet;
            }
            else if(bet.State == BET_STATE.PENDING)
            {
                bool areAllParticipantVoted = this.areAllParticipantVoted(bet.Participations);
                bool isTimeout = TimeUltility.isTimeout(bet.CreationTime, bet.PendingDuration);
                if (areAllParticipantVoted || isTimeout)
                {
                    bet.State = BET_STATE.ANSWERABLE;
                    bet.AnswerStartTime = DateTime.Now;
                    setAllInactiveUserDecline(bet.Participations);
                }
            }
            else if(bet.State == BET_STATE.ANSWERABLE)
            {
                DateTime answerStartTime = bet.AnswerStartTime ?? DateTime.Now;
                bet.AnswerStartTime = answerStartTime;

                bool isTimeout = TimeUltility.isTimeout(answerStartTime, Bet.ANSWER_DURATION);
                if(isTimeout)
                {
                    bet.State = BET_STATE.VERIFYING;
                    bet.VerifyStartTime = DateTime.Now;

                    BetUser creator = getBetUserByUserId(bet.Participations, bet.CreatorId);
                    bet.WinningOption = creator.Option;
                }
            }
            else if(bet.State == BET_STATE.VERIFYING)
            {
                DateTime verifyStartTime = bet.AnswerStartTime ?? DateTime.Now;
                bet.VerifyStartTime = verifyStartTime;

                bool isTimeout = TimeUltility.isTimeout(verifyStartTime, Bet.VERIFY_DURATION);
                bool areAllParticipantAgree = this.areAllParticipantAgree(bet.Participations);

                if(isTimeout || areAllParticipantAgree)
                {
                    bet.State = BET_STATE.FINALLIZED;
                }
            }
            this.dbContext.SaveChanges();
            return bet;
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
                if (betUser.State < BETUSER_STATE.AGREE)
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
    }
}
