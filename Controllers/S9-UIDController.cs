using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using GoogleAnalyticsTracker.Core;
using GoogleAnalyticsTracker.AspNet;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SS_API
{

    [Route("api/[controller]")]
    [ApiController]
    public class S9_UIDController : ControllerBase
    {
        private readonly ILogger<S9_UIDController> _logger;

        public S9_UIDController(ILogger<S9_UIDController> logger)
        {
            _logger = logger;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public string Get()
        {
            return "Hello! Please use the correct format:\"https://matgames.net/api/S9_UID/{category}/{request}\" -Travis" + " " + Directory.GetCurrentDirectory();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}/{request}")]
        public string Get(string id, string request)
        {
            if(System.IO.File.Exists($"/home/pi/sitenine/{id}/{request}.json"))
            {
                User ?temp = JsonConvert.DeserializeObject<User>(System.IO.File.ReadAllText($"/home/pi/sitenine/{id}/{request}.json"));
                temp.PFPLocation = temp.PFPLocation.Remove(0,12);
                temp.PFPLocation = temp.PFPLocation.Insert(0, "https://matgames.net");
                temp.Password = "HIDDEN"; // doesn't seem safe, but in reality i think it is (as it is server side)
                System.IO.File.AppendAllText($"/home/pi/sitenine/logs/{request}.txt", "\n" + JsonConvert.SerializeObject(new AccessdFile("Get")));
                return JsonConvert.SerializeObject(temp).ToString();
            }
            return ""; 
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}/{request}")]
        public async void Put(string id,string request,string function, [FromBody] string value)
        {
            value.Replace("\\",String.Empty);

            

            if (!System.IO.File.Exists($"/home/pi/sitenine/{id}/{request}.json"))
            {
                var newUserProfile = System.IO.File.Create($"/home/pi/sitenine/{id}/{request}.json");
                newUserProfile.Close();
                var newUserActivityLog = System.IO.File.Create($"/home/pi/sitenine/logs/{id}/{request}.txt");
                newUserActivityLog.Close();

                System.IO.File.WriteAllText($"/home/pi/sitenine/logs/{request}.txt", JsonConvert.SerializeObject(new AccessdFile("Create")));
                System.IO.File.WriteAllText($"/home/pi/sitenine/{id}/{request}.json", value);
            }
            else
            {
                User ?inputUser = JsonConvert.DeserializeObject<User>(value);
                User ?storedUser = JsonConvert.DeserializeObject<User>(System.IO.File.ReadAllText($"/home/pi/sitenine/{id}/{request}.json"));
                if (inputUser.Username == storedUser.Username && inputUser.Password == storedUser.Password)
                {
                    System.IO.File.AppendAllText($"/home/pi/sitenine/logs/{request}.txt", "\n" + JsonConvert.SerializeObject(new AccessdFile("Login")));
                    System.IO.File.WriteAllText($"/home/pi/sitenine/{id}/{request}.json", value);
                }
                else
                {
                    System.IO.File.AppendAllText($"/home/pi/sitenine/logs/{request}.txt", "\n" + JsonConvert.SerializeObject(new AccessdFile("LoginFail")));
                    await Task.Delay(1000); //intentional delay to annoy people and definently not to protect against brute force attacks.
                }
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
