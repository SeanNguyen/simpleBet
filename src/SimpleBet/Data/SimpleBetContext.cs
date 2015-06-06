#if DNX451
using SimpleBet.Models;
using System.Data.Entity;

namespace SimpleBet.Data
{
    public class SimpleBetContext : DbContext
    {
        public SimpleBetContext() : base("SimpleBeContext")
        {
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

#endif