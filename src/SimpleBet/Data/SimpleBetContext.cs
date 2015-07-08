using SimpleBet.Models;
using System.Data.Entity;

namespace SimpleBet.Data
{
    public class SimpleBetContext : DbContext
    {

        //use different connection string for different build setting
        //private static string connectionString = @"Data Source=(localdb)\mssqllocaldb;
        //                                        Initial Catalog=SimpleBet;
        //                                        Trusted_Connection=False;
        //                                        Connection Timeout=30;
        //                                        MultipleActiveResultSets=true;";

        private static string connectionString = @"Server=tcp:pvuq1gyimf.database.windows.net,1433;
                                        Database=simplebet; 
                                        User ID=simplebet@pvuq1gyimf;
                                        Password=Simple123;
                                        Trusted_Connection=False;
                                        Encrypt=True;
                                        Connection Timeout=30;
                                        MultipleActiveResultSets=true;";

        public SimpleBetContext() : base(connectionString)
        {
            Database.SetInitializer(new Initializer());
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BetUser> BetUsers { get; set; }
        public DbSet<WinningItem> WinningItems { get; set; }
    }
}