using Microsoft.AspNetCore.Mvc;
using NYoutubeDL;
using System.IO;

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
            Console.WriteLine("Started user-requested download from id: " + id);
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

        private int DownloadVideo(string url)
        {
            var youtubeDl = new YoutubeDL();
            int fCount = Directory.GetFiles("/media/pi/Long-Term/videos/", "*", SearchOption.TopDirectoryOnly).Length;
            string baseUrl = System.IO.File.ReadAllText("/media/pi/Long-Term/keyVideoUrl.txt");
            youtubeDl.Options.FilesystemOptions.Output = $"/media/pi/Long-Term/videos/{fCount+1}.mp4";
            youtubeDl.Options.PostProcessingOptions.ExtractAudio = true;
            youtubeDl.Options.PostProcessingOptions.KeepVideo = true;
            youtubeDl.Options.VideoFormatOptions.FormatAdvanced = "(mp4)[height <=? 720]";
            youtubeDl.VideoUrl = $"{baseUrl}{url}".Trim().Replace(Environment.NewLine,"");

            Console.WriteLine(youtubeDl.VideoUrl);
            // Or update the binary
            youtubeDl.Options.GeneralOptions.Update = true;

            // Optional, required if binary is not in $PATH
            //youtubeDl.YoutubeDlPath = "/usr/local/bin/youtube-dl";

            youtubeDl.StandardOutputEvent += (sender, output) => Console.WriteLine(output);
            youtubeDl.StandardErrorEvent += (sender, errorOutput) => Console.WriteLine(errorOutput);

            youtubeDl.DownloadAsync();
            return fCount+1;
        }
    }
}
