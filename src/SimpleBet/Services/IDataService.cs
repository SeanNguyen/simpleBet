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
        IList<WinningItem> GetNonmonetaryItemsByCreator(int creatorId);
        IList<WinningItem> GetWinningItemsByType(WINNING_ITEM_TYPE type);
        IList<WinningItem> GetMonetaryItemsByCategory(WINNING_ITEM_CATEGORY category);
        WinningItem GetWinningItem(int id);
        WinningItem AddWinningItem(WinningItem item);
        WinningItem UpdateWinningItem(WinningItem item);
        WinningItem RemoveWinningItem(int id);

        //user
        //IList<User> GetUsers();
        User GetUserById(int id);
        User GetUserByFacebookId(long id);
        User AddUser(User user);
        User UpdateUser(User user);
        //User RemoveUser(int id);

        //bet
        //IList<Bet> GetBets();
        Bet GetBet(int id);
        Bet AddBet(Bet bet);
        Bet UpdateBet(Bet bet);
        //Bet RemoveBet(int id);

        //bet
        //IList<BetUser> GetBetUsers();
        BetUser GetBetUser(int betId, int userId);
        //BetUser AddBetUser(BetUser betUser);
        BetUser UpdateBetUser(BetUser betUser);
        BetUser RemoveBetUser(int betId, int userId);
    }
}
