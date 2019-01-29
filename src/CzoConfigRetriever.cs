using System.IO;
using Newtonsoft.Json;

namespace Chorizo
{
    public class CzoConfigRetriever : IConfigRetriever
    {
        public readonly IFileReader FileReader;

        public CzoConfigRetriever(IFileReader fileReader = null, string path = null)
        {
            var filePath = path ?? "config/CzoConfig.json";
            FileReader = fileReader ?? new DotNetFileReader(filePath);
        }
        
        public ServerConfig GetConfig()
        {
            using (var file = FileReader)
            {
                var json = file.ReadToEnd();
                var config = JsonConvert.DeserializeObject<ServerConfig>(json);
                return config;
            }
        }
    }
}