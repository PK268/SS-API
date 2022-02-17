using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;

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
            if (id != "u")
            {
                return "Invalid ID specifier: \"" + id + "\". Please use the correct format.";
            }

            if (Regex.IsMatch(request, @"[,/\\.]"))
            {
                return "Invalid Request: " + request + ". Please use the correct format.";
            }
            
            if (System.IO.File.Exists($"/home/pi/sitenine/{id}/{request}.json"))
            {
                User? temp = JsonConvert.DeserializeObject<User>(System.IO.File.ReadAllText($"/home/pi/sitenine/{id}/{request}.json"));
                temp.PFPLocation = temp.PFPLocation.Remove(0, 12); //Removing filepath
                temp.PFPLocation = temp.PFPLocation.Insert(0, "https://matgames.net"); //Making it an accessable URL
                temp.Password = "HIDDEN"; // Doesn't seem safe, but in reality I THINK it is (as it is server side)
                System.IO.File.AppendAllText($"/home/pi/sitenine/logs/{request}.txt", "\n" + JsonConvert.SerializeObject(new AccessdFile("Get"))); //Log
                return JsonConvert.SerializeObject(temp).ToString();
            }
            return $"User not found: {request} of type: {id}";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}/{request}")]
        public async void Put(string id, string request, string function, [FromBody] string value)
        {
            if (id != "u" || Regex.IsMatch(request, @"[,/\\.]"))
            {
                return;
            }
            
            if (allowRequest(request))
            {
                value.Replace("\\", String.Empty); //Re formats stirng from transport

                if (!System.IO.File.Exists($"/home/pi/sitenine/{id}/{request}.json")) //Create new user
                {
                    var newUserProfile = System.IO.File.Create($"/home/pi/sitenine/{id}/{request}.json");
                    newUserProfile.Close();
                    var newUserActivityLog = System.IO.File.Create($"/home/pi/sitenine/logs/{id}/{request}.txt");
                    newUserActivityLog.Close();

                    System.IO.File.WriteAllText($"/home/pi/sitenine/logs/{request}.txt", JsonConvert.SerializeObject(new AccessdFile("Create"))); //Create new log file
                    System.IO.File.WriteAllText($"/home/pi/sitenine/{id}/{request}.json", value); //Create new user file
                }
                else //Edit existing user
                {
                    User? inputUser = deserializeInput(value);
                    User? storedUser = deserializeInput(System.IO.File.ReadAllText($"/home/pi/sitenine/{id}/{request}.json"));

                    inputUser.DateCreated = storedUser.DateCreated; //Ensures DateCreated can't be changed

                    if (inputUser.Username == storedUser.Username && inputUser.Password == storedUser.Password) //If creds check out
                    {
                        System.IO.File.AppendAllText($"/home/pi/sitenine/logs/{request}.txt", "\n" + JsonConvert.SerializeObject(new AccessdFile("Login")));
                        System.IO.File.WriteAllText($"/home/pi/sitenine/{id}/{request}.json", value);
                    }
                    else
                    {
                        System.IO.File.AppendAllText($"/home/pi/sitenine/logs/{request}.txt", "\n" + JsonConvert.SerializeObject(new AccessdFile("LoginFail")));
                    }
                }
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}/{request}")]
        public async void Delete(string id, string request, [FromBody] string value)
        {
            if (id != "u" || Regex.IsMatch(request, @"[,/\\.]"))
            {
                return;
            }
            
            if (allowRequest(request))
            {
                value.Replace("\\", String.Empty); //Re formats stirng from transport

                if (System.IO.File.Exists($"/home/pi/sitenine/{id}/{request}.json")) //Create new user
                {
                    User? inputUser = deserializeInput(value);
                    User? storedUser = deserializeInput(System.IO.File.ReadAllText($"/home/pi/sitenine/{id}/{request}.json"));

                    if (inputUser.Username == storedUser.Username && inputUser.Password == storedUser.Password) //If creds check out
                    {
                        System.IO.File.Delete($"/home/pi/sitenine/{id}/{request}.json");
                    }
                    else
                    {
                        System.IO.File.AppendAllText($"/home/pi/sitenine/logs/{request}.txt", "\n" + JsonConvert.SerializeObject(new AccessdFile("LoginFailDelete")));
                    }
                }
            }
        }
        /// <summary>
        /// Generates a MemoryStream from a string (copied from you know where)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Converted stream</returns>
        static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        /// <summary>
        /// Takes in request (name of user) and determines weather or not to let a request go through.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Weather or not to allow the request.</returns>
        bool allowRequest(string request)
        {
            var log = System.IO.File.ReadLines($"/home/pi/sitenine/logs/{request}.txt");

            int logLineCount = log.Count();

            string logContents = log.Skip(logLineCount - 1).Take(1).First();

            AccessdFile? fromLog = JsonConvert.DeserializeObject<AccessdFile>(logContents);

            if (DateTimeOffset.Now.ToUnixTimeSeconds() - fromLog.UnixTime > 10)
            {
                return true;
            }

            System.IO.File.AppendAllText($"/home/pi/sitenine/logs/{request}.txt", "\n" + JsonConvert.SerializeObject(new AccessdFile("LoginDeny")));
            return false;
        }

        User deserializeInput(string value)
        {
            var ds = new DataContractJsonSerializer(typeof(User));
            return (User)ds.ReadObject(GenerateStreamFromString(value));
        }
    }
}
