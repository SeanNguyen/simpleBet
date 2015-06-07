using SimpleBet.Models;
using System.Data.Entity;
using System.Data.SqlClient;

namespace SimpleBet.Data
{
    public class SimpleBetContext : DbContext
    {
        public SimpleBetContext() : base(@"Data Source=(localdb)\mssqllocaldb;
                                          Initial Catalog=SimpleBet;
                                          Integrated Security=True; Pooling=false;
                                            MultipleActiveResultSets=true;")
        {
            Database.SetInitializer<SimpleBetContext>(new Initializer());
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Option> Options { get; set; }
    }
}