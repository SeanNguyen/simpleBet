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
            int it = betController.Post(bet);
        }
    }
}
