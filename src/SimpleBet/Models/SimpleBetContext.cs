using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Models
{
    public class SimpleBetContext : DbContext
    {
        public SimpleBetContext()
        {
        }

        public DbSet<ValueModel> Values { get; set; }
    }
}   