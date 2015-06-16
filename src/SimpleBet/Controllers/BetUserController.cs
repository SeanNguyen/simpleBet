using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimpleBet.Controllers
{
    //[Route("api/[controller]")]
    public class BetUserController : ApiController
    {
        //Attributes
        private readonly SimpleBetContext dbContext;

        //Constructors
        public BetUserController()
        {
            this.dbContext = new SimpleBetContext();
        }

        // GET: api/values
        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(this.dbContext.BetUsers);
        }

        //currently cannot get by Id will add later if required
        // GET api/values/5
        //[HttpGet("{id:int}")]
        //public IActionResult Get(int id)
        //{
        //    BetUser user = this.dbContext.BetUsers.Find(id);
        //    if (user == null)
        //    {
        //        return new HttpNotFoundResult();
        //    }
        //    else
        //    {
        //        string userJson = JsonConvert.SerializeObject(user);
        //        return new ObjectResult(userJson);
        //    }
        //}
        
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]BetUser betUser)
        {
            this.dbContext.BetUsers.Add(betUser);
            this.dbContext.SaveChanges();
            string json = JsonConvert.SerializeObject(betUser);
            return new ObjectResult(json);
        }

        // PUT api/values/5
        [Route("api/[controller]/{betId}/{userId}")]
        [HttpPut]
        public async Task<IActionResult> Put(int betId, int userId, [FromBody]BetUser betUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (betId != betUser.BetId || userId != betUser.UserId)
            {
                return BadRequest();
            }

            this.dbContext.Entry(betUser).State = EntityState.Modified;

            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!isExist(betId, userId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //post-process bet
            updateCancellingStatus(betId);
            updateFinallizableStatus(betId);

            //seriallize the object
            string json = JsonConvert.SerializeObject(betUser);
            return Ok(json);
        }

        // DELETE api/values/5
        [Route("api/[controller]/{betId}/{userId}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int userId, int betId)
        {
            BetUser betUser = this.dbContext.BetUsers
                                .Where(bu => bu.BetId == betId && bu.UserId == userId)
                                .FirstOrDefault();
            if (betUser == null)
            {
                return NotFound();
            }

            this.dbContext.BetUsers.Remove(betUser);
            await this.dbContext.SaveChangesAsync();

            string json = JsonConvert.SerializeObject(betUser);
            return Ok(json);
        }

        //private helper methods
        private bool isExist(int betId, int userId)
        {
            return this.dbContext.BetUsers.Count(b => b.BetId == betId && b.UserId == userId) > 0;
        }

        private void updateCancellingStatus(int betId)
        {
            Bet bet = this.dbContext.Bets.Find(betId);
            int agreeCount = 0;
            int disagreeCount = 0;
            for (int i = 0; i < bet.Participations.Count; i++)
            {
                BetUser participation = bet.Participations.ElementAt(i);
                if (participation.VoteCancelBetState == VoteCancelBetState.DISAGREE)
                {
                    disagreeCount++;
                }
                else if (participation.VoteCancelBetState == VoteCancelBetState.AGREE
                        || participation.VoteCancelBetState == VoteCancelBetState.CREATOR)
                {
                    agreeCount++;
                }
            }
            if (agreeCount * 2 >= bet.Participations.Count)
            {
                bet.State = BET_STATE.CANCELLED;
            }
            else if (disagreeCount * 2 >= bet.Participations.Count)
            {
                bet.State = BET_STATE.CONFIRM;
                //reset all the edges
                for (int i = 0; i < bet.Participations.Count; i++)
                {
                    BetUser participation = bet.Participations.ElementAt(i);
                    participation.VoteCancelBetState = VoteCancelBetState.NONE;
                }
            }
            this.dbContext.SaveChanges();
        }

        private void updateFinallizableStatus(int betId)
        {
            Bet bet = this.dbContext.Bets.Find(betId);
            bool allAnswered = true;
            for (int i = 0; i < bet.Participations.Count; i++)
            {
                BetUser participation = bet.Participations.ElementAt(i);
                if (participation.State < BetUserState.VOTED)
                {
                    allAnswered = false;
                    break;
                }
            }
            if(allAnswered)
            {
                bet.State = BET_STATE.FINALLIZABLE;
            }
            this.dbContext.SaveChanges();
        }
    }
}
