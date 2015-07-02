using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using SimpleBet.Services;
using System.Linq;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        //Attributes
        private IDataService dataService = new DataService(new SimpleBetContext());

        //Constructors
        public UserController() { }

        // GET: api/values
        //[HttpGet]
        //public string Get()
        //{
        //    return JsonConvert.SerializeObject(this.dataService.GetUsers());
        //}

        // GET api/values/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            User user = this.dataService.GetUserById(id);
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
            User user = this.dataService.GetUserByFacebookId(id);
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
        public IActionResult Post([FromBody]User user)
        {
            user = this.dataService.AddUser(user);
            string json = JsonConvert.SerializeObject(user);
            return new ObjectResult(json);
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