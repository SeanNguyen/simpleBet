using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SimpleBet.Controllers;
using SimpleBet.Models;

namespace SimpleBet.Test.Api
{
    public class BetControllerTest
    {
        [Fact]
        public void Create()
        {
            BetController betController = new BetController();
            Bet bet = new Bet();
            bet.Question = "This is a question";
            bet.CreationDate = DateTime.Now;
            bet.Duration = 60;
            bet.CreatorId = 1;
            betController.Post(bet);

            UserController userController = new UserController();
            User user = new User();
            user.Name = "Sean";
            user.Bets.Add(bet);
            userController.Post(user);

        }
    }
}
