using SimpleBet.Models;
using System.Data.Entity;

namespace SimpleBet.Data
{
    public class SimpleBetContext : DbContext
    {
        public SimpleBetContext() : base(@"Data Source=(localdb)\mssqllocaldb;
                                          Initial Catalog=SimpleBet;
                                          Integrated Security=True;")
        {
            Database.SetInitializer<SimpleBetContext>(new Initializer());
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}