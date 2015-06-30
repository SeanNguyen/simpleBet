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
    }
}
