﻿using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace SimpleBet.Data
{

    public class Initializer : DropCreateDatabaseIfModelChanges<SimpleBetContext>
    {
        protected override void Seed(SimpleBetContext context)
        {
            //Users
            context.Users.Add(new User() { FacebookId = 123, Name = "Admin", AvatarUrl = "http://png-1.findicons.com/files/icons/1072/face_avatars/300/i04.png" });
            context.Users.Add(new User() { FacebookId = 321, Name = "I am a robot", AvatarUrl = "someUrl" });
            context.SaveChanges();

            //Winning Items
            context.WinningItems.Add(new WinningItem()
            {
                Type = WINNING_ITEM_TYPE.NONMONETARY,
                Title = "Punch",
                Description = "I will punch you",
                ImageUrl = "assets/icon_dare.png",
                CreatorId = 1
            });
            context.WinningItems.Add(new WinningItem()
            {
                Type = WINNING_ITEM_TYPE.MONETARY,
                Category = WINNING_ITEM_CATEGORY.WINE,
                Title = "Wine",
                Description = "I will buy you this",
                ImageUrl = "assets/icon_giftBox.png",
                CreatorId = 1,
                Price = 50.2
            });
            context.WinningItems.Add(new WinningItem()
            {
                Type = WINNING_ITEM_TYPE.NONMONETARY,
                Title = "Catch‘em All",
                Description = "Put on your business suit and seal the nearest 3 kids you see into a ball. (They must wear a tie.)",
                ImageUrl = "assets/icon_dare.png",
                CreatorId = 1
            });
            context.WinningItems.Add(new WinningItem()
            {
                Type = WINNING_ITEM_TYPE.NONMONETARY,
                Title = "A Noble King",
                Description = "Donate all of your valubles in your wallet to a nearby beggar.",
                ImageUrl = "assets/icon_dare.png",
                CreatorId = 1
            });
            context.WinningItems.Add(new WinningItem()
            {
                Type = WINNING_ITEM_TYPE.NONMONETARY,
                Title = "Static Shock",
                Description = "Get eletricfied in a thunderstorm. Zap Zap pikachu I choose you.",
                ImageUrl = "assets/icon_dare.png",
                CreatorId = 1
            });
            context.SaveChanges();

            //Bets
            List<Option> options = new List<Option>();
            options.Add(new Option() { Content = "This is option a" });
            options.Add(new Option() { Content = "This is option b" });

            List<BetUser> participations = new List<BetUser>();
            participations.Add(new BetUser() { State = BETUSER_STATE.CONFIRMED, UserId = 1 });
            participations.Add(new BetUser() { State = BETUSER_STATE.PENDING, UserId = 2 });

            context.Bets.Add(new Bet()
            {
                BetType = BET_TYPE.ONE_MANY,
                CreationTime = DateTime.Now,
                PendingDuration = 1000,
                CreatorId = 1,
                Question = "This is a question",
                State = BET_STATE.PENDING,
                Options = options,
                Participations = participations,
                WinningItemId = 1
            });

            context.Bets.Add(new Bet()
            {
                BetType = BET_TYPE.ONE_MANY,
                CreationTime = DateTime.Now,
                PendingDuration = 1000,
                CreatorId = 1,
                Question = "This is a question",
                State = BET_STATE.ANSWERABLE,
                WinningItemId = 1
            });

            context.Bets.Add(new Bet()
            {
                BetType = BET_TYPE.ONE_MANY,
                CreationTime = DateTime.Now,
                PendingDuration = 1000,
                CreatorId = 1,
                Question = "This is a question",
                State = BET_STATE.VERIFYING,
                WinningItemId = 1
            });
            context.SaveChanges();

            base.Seed(context);
        }
    }

}