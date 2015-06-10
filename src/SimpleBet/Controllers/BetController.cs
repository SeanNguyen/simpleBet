using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using System;
using System.Collections.Generic;

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
            Bet bet = this.dbContext.Bets.Find(id);
            if (bet == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                string betJson = JsonConvert.SerializeObject(bet);
                return new ObjectResult(betJson);
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Bet bet)
        {
            bet.CreationDate = DateTime.Now;
            this.dbContext.Bets.Add(bet);
            this.dbContext.SaveChanges();
            string betJson = JsonConvert.SerializeObject(bet);
            return new ObjectResult(betJson);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}