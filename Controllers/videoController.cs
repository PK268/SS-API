using Microsoft.AspNetCore.Mvc;
using NYoutubeDL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class videoController : ControllerBase
    {
        // GET: api/<videoController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<videoController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            int fcount = DownloadVideo(id);
            return $"https://matgames.net/LTS/videos/{fcount}.mp4";
        }

        // POST api/<videoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<videoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<videoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public int DownloadVideo(string url)
        {
            var youtubeDl = new YoutubeDL();
            int fCount = Directory.GetFiles("/media/pi/Long-Term/videos/", "*", SearchOption.TopDirectoryOnly).Length;
            
            youtubeDl.Options.FilesystemOptions.Output = $"/media/pi/Long-Term/videos/{fCount+1}.mp4";
            youtubeDl.Options.PostProcessingOptions.ExtractAudio = true;
            youtubeDl.VideoUrl = url;

            // Or update the binary
            youtubeDl.Options.GeneralOptions.Update = true;

            // Optional, required if binary is not in $PATH
            youtubeDl.YoutubeDlPath = "/usr/local/bin/youtube-dl";
            youtubeDl.DownloadAsync();
            return fCount+1;
        }
    }
}
