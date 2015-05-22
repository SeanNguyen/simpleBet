using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SimpleBet.Models;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        //Attributes
        private readonly SimpleBetContext dbContext;

        //Constructors
        public ValuesController (SimpleBetContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<ValueModel> Get()
        {
            return this.dbContext.Values;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ValueModel value = this.dbContext.Values.FirstOrDefault(v => v.Id == id);
            if (value == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                return new ObjectResult(value);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
