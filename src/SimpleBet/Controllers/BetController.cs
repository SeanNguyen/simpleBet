using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
    public class BetController : ApiController
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

            if (bet == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                //TODO: later change this to Eager Loading
                //foreach (BetUser betUser in bet.Participations)
                //{
                //    User mock = betUser.User;
                //}
                string betJson = JsonConvert.SerializeObject(bet);
                return new ObjectResult(betJson);
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Bet bet)
        {
            //manually create datetime here, TODO: maybe move this creation to client
            bet.CreationTime = DateTime.Now;

            this.dbContext.Bets.Add(bet);
            this.dbContext.SaveChanges();
            string betJson = JsonConvert.SerializeObject(bet);
            return new ObjectResult(betJson);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Bet bet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != bet.Id)
            {
                return BadRequest();
            }
            if (!isExist(id))
            {
                return NotFound();
            }

            Bet existingBet = this.dbContext.Bets.Find(id);
            this.dbContext.Entry(existingBet).CurrentValues.SetValues(bet);

            //update all participations as well
            //remove
            foreach (BetUser existingBetUser in existingBet.Participations.ToList())
            {  
                if (!bet.Participations.Any(p => p.UserId == existingBetUser.UserId))
                {
                    existingBet.Participations.Remove(existingBetUser);
                }
            }
            //add and update
            foreach(BetUser updatedBetUser in bet.Participations.ToList())
            {
                BetUser existingParticipation = existingBet.Participations.FirstOrDefault(p => p.UserId == updatedBetUser.UserId);
                //if existed then update it
                if (existingParticipation != null)
                {
                    this.dbContext.Entry(existingParticipation).CurrentValues.SetValues(updatedBetUser);
                }
                else //if not, add it
                {
                    existingBet.Participations.Add(updatedBetUser);
                }
            }

            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            //seriallize the object
            string json = JsonConvert.SerializeObject(bet);
            return Ok(json);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Bet bet = await this.dbContext.Bets.FindAsync(id);
            if (bet == null)
            {
                return NotFound();
            }

            this.dbContext.Bets.Remove(bet);
            await this.dbContext.SaveChangesAsync();

            return Ok(bet);
        }

        //private helper methods
        private bool isExist(int id)
        {
            return this.dbContext.Bets.Count(b => b.Id == id) > 0;
        }

    }
}