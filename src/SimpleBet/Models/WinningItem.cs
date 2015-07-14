﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Models
{
    public enum WINNING_ITEM_TYPE
    {
        NONE,
        MONETARY,
        NONMONETARY
    }

    public enum WINNING_ITEM_CATEGORY
    {
        NONE,
        VOUCHER,
        BEAUTY,
        FNB,
        WINE,
        BEER
    }

    public class WinningItem : Model
    {
        public WinningItem()
        {
            this.Bets = new List<Bet>();
        }

        public int Id { get; set; }
        public WINNING_ITEM_TYPE Type { get; set; }
        public WINNING_ITEM_CATEGORY Category { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<Bet> Bets { get; set; }
        
        //public methods
        public override Model parse(dynamic data)
        {
            throw new NotImplementedException();
        }

        public override Model stringlify()
        {
            throw new NotImplementedException();
        }
    }
}
