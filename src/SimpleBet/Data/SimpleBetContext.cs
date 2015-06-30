using SimpleBet.Models;
using System.Data.Entity;

/* Connection string for azure server
@"Server=tcp:pvuq1gyimf.database.windows.net,1433;Database=simplebet;
                                        User ID=simplebet@pvuq1gyimf;Password=Simple123;
                                        Trusted_Connection=False;
                                        Encrypt=True;
                                        Connection Timeout=30;
                                        MultipleActiveResultSets=true;"
*/

namespace SimpleBet.Data
{
    public class SimpleBetContext : DbContext
    {
        public SimpleBetContext() : base(@"Data Source=(localdb)\mssqllocaldb;
                                        Initial Catalog=SimpleBet;
                                        Integrated Security=True; Pooling=false;
                                        MultipleActiveResultSets=true;")
        {
            Database.SetInitializer(new Initializer());
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BetUser> BetUsers { get; set; }
        public DbSet<WinningItem> WinningItems { get; set; }
    }
}