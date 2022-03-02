using Microsoft.AspNetCore.Mvc;

using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneController : ControllerBase
    {
        private readonly ILogger<GeneController> _logger;
        private static List<Gene> geneData = new List<Gene>();

        public GeneController(ILogger<GeneController> logger)
        {
            _logger = logger;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            if (geneData.Count == 0)
            {
                string readData = System.IO.File.ReadAllText("/home/pi/gene/geneData.json");
                if (readData != "")
                {
                    geneData = deserializeInput(readData);
                }
            }

            if (id == "data")
            {
                string combinedData = "";
                foreach (Gene gene in geneData)
                {
                    combinedData += $"{gene.Symbol},{gene.Acid},{gene.Function}\n";
                }
                return combinedData;
            }

            foreach (Gene gene in geneData)
            {
                if (gene.Symbol == id)
                {
                    return $"{gene.Symbol}\n{gene.Acid}\n{gene.Function}";
                }
            }

            return "no gene found";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}/{username}/{password}")]
        public void Put(string id, string username, string password, [FromBody] string value)
        {
            string[] seperated = id.Split(';');
            Gene temp = new Gene(seperated[0], seperated[1], seperated[2]);

            if (username == System.IO.File.ReadAllText("/home/pi/gene/adminUsername.txt").Trim() && password == System.IO.File.ReadAllText("/home/pi/gene/adminPassword.txt").Trim())
            {
                geneData.Add(temp);
                System.IO.File.WriteAllText("/home/pi/gene/geneData.json", JsonConvert.SerializeObject(geneData));
                //geneData = deserializeInput(value);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        List<Gene> deserializeInput(string value)
        {
            var ds = new DataContractJsonSerializer(typeof(List<Gene>));
            return (List<Gene>)ds.ReadObject(GenerateStreamFromString(value));
        }
    }
}
