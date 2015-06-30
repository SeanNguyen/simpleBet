using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Services
{
    public interface IDataService
    {
        //winning Item
        IList<WinningItem> GetWinningItems();
        WinningItem GetWinningItem(int id);
        WinningItem AddWinningItem(WinningItem item);
        WinningItem UpdateWinningItem(WinningItem item);
        WinningItem RemoveWinningItem(int id);
    }
}
