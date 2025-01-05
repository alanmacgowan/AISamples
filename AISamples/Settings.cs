using System.Reflection.Metadata;
using System.Reflection;
using System.Text.Json;
using System.Diagnostics;

namespace AISamples
{

    public static class ApplicationSettings
    {

        public static (string apiKey, string? orgId) LoadFromEnvironment()
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

                if (apiKey == null)
                {
                    Console.WriteLine("apiKey is null");
                    throw new Exception("apiKey is null");
                }

                var orgId = Environment.GetEnvironmentVariable("OPENAI_ORG_ID");

                return (apiKey, orgId);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: " + e.Message);
                return ("", "");
            }
        }

        public static (string apiKey, string? orgId) LoadFromFile(string configFile = "settings.json")
        {

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), configFile);

            if (!File.Exists(path))
            {
                Console.WriteLine("Configuration not found: " + path);
                throw new Exception("Configuration not found");
            }
            try
            {
                var config = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));

                // check whether config is null
                if (config == null)
                {
                    Console.WriteLine("Configuration is null");
                    throw new Exception("Configuration is null");
                }

                string apiKey = config["apiKey"];


                string? orgId;

                // check whether orgId is in the file
                if (!config.ContainsKey("orgId"))
                {
                    orgId = null;
                }
                else
                {
                    orgId = config["orgId"];
                }
                return (apiKey, orgId);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: " + e.Message);
                return ("", "");
            }
        }

        public static string GetPluginsDirectory(string baseFolder, string pluginFolder = "")
        {
            return $"SemanticKernel/{baseFolder}/Plugins/{pluginFolder}";
        }
    }
}
