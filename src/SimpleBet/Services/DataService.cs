using SimpleBet.Data;
using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Services
{
    public class DataService : IDataService
    {
        private readonly SimpleBetContext dbContext = new SimpleBetContext();

        public DataService() { }

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
            Bet existingBet = this.dbContext.Bets.Find(bet.Id);
            dbContext.Entry(existingBet).CurrentValues.SetValues(bet);

            //save all the connection as well
            for(int i = 0; i < bet.Participations.Count; i++)
            {
                BetUser betUser = bet.Participations.ElementAt(i);
                if(dbContext.BetUsers.Count(bu => bu.BetId == betUser.BetId && bu.UserId == betUser.UserId) > 0)
                {
                    dbContext.Entry(betUser).State = EntityState.Modified;
                }
                else
                {
                    dbContext.Entry(betUser).State = EntityState.Added;
                }
            }

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
            updateCancellingStatus(betUser.BetId);
            updateFinallizableStatus(betUser.BetId);

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

        private void updateCancellingStatus(int betId)
        {
            Bet bet = this.dbContext.Bets.Find(betId);
            int agreeCount = 0;
            int disagreeCount = 0;
            for (int i = 0; i < bet.Participations.Count; i++)
            {
                BetUser participation = bet.Participations.ElementAt(i);
                if (participation.VoteCancelBetState == VoteCancelBetState.DISAGREE)
                {
                    disagreeCount++;
                }
                else if (participation.VoteCancelBetState == VoteCancelBetState.AGREE
                        || participation.VoteCancelBetState == VoteCancelBetState.CREATOR)
                {
                    agreeCount++;
                }
            }
            if (agreeCount * 2 >= bet.Participations.Count)
            {
                bet.State = BET_STATE.CANCELLED;
            }
            else if (disagreeCount * 2 >= bet.Participations.Count)
            {
                bet.State = BET_STATE.CONFIRM;
                //reset all the edges
                for (int i = 0; i < bet.Participations.Count; i++)
                {
                    BetUser participation = bet.Participations.ElementAt(i);
                    participation.VoteCancelBetState = VoteCancelBetState.NONE;
                }
            }
            this.dbContext.SaveChanges();
        }

        private void updateFinallizableStatus(int betId)
        {
            Bet bet = this.dbContext.Bets.Find(betId);
            bool allAnswered = true;
            for (int i = 0; i < bet.Participations.Count; i++)
            {
                BetUser participation = bet.Participations.ElementAt(i);
                if (participation.State < BetUserState.VOTED)
                {
                    allAnswered = false;
                    break;
                }
            }
            if (allAnswered)
            {
                bet.State = BET_STATE.FINALLIZABLE;
            }
            this.dbContext.SaveChanges();
        }
    }
}
