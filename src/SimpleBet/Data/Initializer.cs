using SimpleBet.Models;
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
            context.Users.Add(new User() { FacebookId = 123, Name = "Jon Snow", AvatarUrl = "someUrl" });
            context.Users.Add(new User() { FacebookId = 321, Name = "I am a robot", AvatarUrl = "someUrl" });
            context.SaveChanges();

            //Winning Items
            context.WinningItems.Add(new WinningItem()
            {
                Type = WINNING_ITEM_TYPE.NONMONETARY,
                Title = "Punch",
                Description = "I will punch you",
                CreatorId = 1
            });
            context.WinningItems.Add(new WinningItem()
            {
                Type = WINNING_ITEM_TYPE.MONETARY,
                Title = "Wine",
                Description = "I will buy you this",
                CreatorId = 2
            });
            context.SaveChanges();

            //Bets
            List<Option> options = new List<Option>();
            options.Add(new Option() { Content = "This is option a" });
            options.Add(new Option() { Content = "This is option b" });

            List<BetUser> participations = new List<BetUser>();
            participations.Add(new BetUser() { State = BetUserState.CONFIRMED, UserId = 1 });
            participations.Add(new BetUser() { State = BetUserState.PENDING, UserId = 2 });

            context.Bets.Add(new Bet()
            {
                BetType = BET_TYPE.ONE_MANY,
                CreationTime = DateTime.Now,
                Duration = 1000,
                CreatorId = 1,
                Question = "This is a question",
                State = BET_STATE.CONFIRM,
                Options = options,
                Participations = participations,
                //WinningItemId = 1
            });
            context.SaveChanges();

            base.Seed(context);
        }
    }

}