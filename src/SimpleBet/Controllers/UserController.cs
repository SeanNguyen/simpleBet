using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using System.Linq;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        //Attributes
        private readonly SimpleBetContext dbContext;

        //Constructors
        public UserController()
        {
            this.dbContext = new SimpleBetContext();
        }

        // GET: api/values
        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(this.dbContext.Users);
        }

        // GET api/values/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            User user = this.dbContext.Users.Find(id);
            if (user == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                string userJson = JsonConvert.SerializeObject(user);
                return new ObjectResult(userJson);
            }
        }

        //this will be hitted when querry by facebookID
        [HttpGet("{id:long}")]
        public IActionResult Get(long id)
        {
            User user = this.dbContext.Users.FirstOrDefault(u => u.FacebookId == id);
            if (user == null)
            {
                return new HttpNotFoundResult();
            } else
            {
                string userJson = JsonConvert.SerializeObject(user);
                return new ObjectResult(userJson);
            }
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody]User user)
        {
            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();
            return user.Id;
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