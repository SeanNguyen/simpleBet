using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using SimpleBet.Services;
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
    public class WinningItemController : ApiController
    {
        //Attributes
        private IDataService dataService = new DataService(new SimpleBetContext());

        //Constructors
        public WinningItemController() { }

        // GET: api/values
        [HttpGet]
        public IActionResult Query([FromUri] int creatorId)
        {
            IList<WinningItem> winningItems;
            if (creatorId > 0)
            {
                winningItems = dataService.GetWinningItemsByCreator(creatorId);
            }
            else
            {
                winningItems = dataService.GetWinningItems();
            }
            string json = JsonConvert.SerializeObject(winningItems);
            return Ok(json);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            WinningItem winningItem = dataService.GetWinningItem(id);

            if (winningItem == null)
            {
                return new HttpNotFoundResult();
            }
            string json = JsonConvert.SerializeObject(winningItem);
            return Ok(json);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]WinningItem winningItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dataService.AddWinningItem(winningItem);
            string json = JsonConvert.SerializeObject(winningItem);
            return Ok(json);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]WinningItem winningItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != winningItem.Id)
            {
                return BadRequest();
            }

            try
            {
                this.dataService.UpdateWinningItem(winningItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (dataService.GetWinningItem(id) != null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            string json = JsonConvert.SerializeObject(winningItem);
            return Ok(json);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                WinningItem winningItem = this.dataService.RemoveWinningItem(id);
                string json = JsonConvert.SerializeObject(winningItem);
                return Ok(json);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
