using Microsoft.AspNetCore.Mvc;

using System.Runtime.Serialization.Json;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneController : ControllerBase
    {

        private readonly ILogger<GeneController> _logger;
        private List<Gene> geneData;

        public GeneController(ILogger<GeneController> logger)
        {
            _logger = logger;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            if(geneData == null)
            {
                geneData = deserializeInput(System.IO.File.ReadAllText("~/gene/geneData.json"));
            }
            
            foreach(Gene gene in geneData)
            {
                if(gene.Symbol == id)
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
        public void Put(string id, string username, string password,[FromBody] string value)
        {
            if (username == System.IO.File.ReadAllText("~/gene/adminUSername.txt") && password == System.IO.File.ReadAllText("~/gene/adminPassword.txt"))
            {
                System.IO.File.WriteAllText("~/gene/geneData.json",value);
                geneData = deserializeInput(value);
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
