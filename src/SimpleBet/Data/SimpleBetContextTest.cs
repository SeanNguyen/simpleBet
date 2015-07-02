using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Data
{
    public class SimpleBetContextTest : DbContext
    {
        public SimpleBetContextTest() : base(@"Data Source=(localdb)\mssqllocaldb;
                                        Initial Catalog=SimpleBet;
                                        Integrated Security=True; Pooling=false;
                                        MultipleActiveResultSets=true;")
        {
            Database.SetInitializer(new InitializerTest());
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BetUser> BetUsers { get; set; }
        public DbSet<WinningItem> WinningItems { get; set; }
    }
}
