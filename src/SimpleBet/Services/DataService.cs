using SimpleBet.Data;
using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Services
{
    public class DataService :IDataService
    {
        private readonly SimpleBetContext dbContext = new SimpleBetContext();

        public DataService() { }

        //Winning Item
        public IList<WinningItem> GetWinningItems()
        {
            return dbContext.WinningItems.ToList();
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
            return dbContext.WinningItems.ToList();
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
    }
}
