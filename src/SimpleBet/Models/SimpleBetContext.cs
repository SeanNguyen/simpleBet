﻿using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Models
{
    public class SimpleBetContext : DbContext
    {
        public SimpleBetContext()
        {
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}   