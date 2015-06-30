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

        //user
        IList<User> GetUsers();
        User GetUserById(int id);
        User GetUserByFacebookId(long id);
        User AddUser(User user);
        User UpdateUser(User user);
        User RemoveUser(int id);
    }
}
