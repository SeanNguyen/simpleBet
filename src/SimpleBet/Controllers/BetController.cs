using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
    public class BetController
    {
        //Attributes
        private readonly SimpleBetContext dbContext;

        //Constructors
        public BetController()
        {
            this.dbContext = new SimpleBetContext();
        }

        // GET: api/values
        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(this.dbContext.Bets);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Bet bet = this.dbContext.Bets.Where(b => b.Id == id)
                                        .Include(b => b.Participations.Select(p => p.User))
                                        .FirstOrDefault();
            //Bet bet = this.dbContext.Bets.Find(id);
            if (bet == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                //TODO: later change this to Eager Loading
                foreach (BetUser betUser in bet.Participations)
                {
                    User mock = betUser.User;
                }
                string betJson = JsonConvert.SerializeObject(bet);
                return new ObjectResult(betJson);
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Bet bet)
        {
            bet.CreationDate = DateTime.Now;
            //Check if user alr in db or not then place it with the one in db
            //foreach (User user in this.dbContext.Users)
            //{
            //    foreach (BetUser betUser in bet.Participations)
            //    {
            //        //check Id, tagId, facebook id
            //        if ((betUser.User.Id != -1 && user.Id == betUser.User.Id)
            //            || (betUser.User.TagId != null && user.TagId == betUser.User.TagId) 
            //            || (betUser.User.FacebookId != -1 && user.FacebookId == betUser.User.FacebookId))
            //        {
            //            betUser.User = user;
            //        }
            //    }
            //}
            this.dbContext.Bets.Add(bet);
            this.dbContext.SaveChanges();
            string betJson = JsonConvert.SerializeObject(bet);
            return new ObjectResult(betJson);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Bet bet)
        {
            Bet existingBet = this.dbContext.Bets.Where(b => b.Id == id).FirstOrDefault();
            if(existingBet != null)
            {
                this.dbContext.
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}