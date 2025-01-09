using Microsoft.SemanticKernel;

namespace AISamples.Dotnet.SemanticKernel.ChainOfThought
{
    public static class ChainOfThought
    {

        public static async Task RunSample()
        {
            var (apiKey, orgId) = ApplicationSettings.LoadFromEnvironment();

            Kernel kernel = Kernel.CreateBuilder()
                                  .AddOpenAIChatCompletion("gpt-3.5-turbo", apiKey, orgId, serviceId: "gpt35")
                                  .Build();


            var promptPlugin = kernel.ImportPluginFromPromptDirectory(ApplicationSettings.GetPluginsDirectory("ChainOfThought", "prompt_engeneering"));

            var problem = "When I was 6 my sister was half my age. Now I'm 70. How old is my sister?";

            var results = new List<int>();

            for (int i = 0; i < 7; i++)
            {
                var chatFunctionVariables1 = new KernelArguments()
                {
                    ["problem"] = problem,
                };

                var steps = await kernel.InvokeAsync(promptPlugin["solve_math_problem"], chatFunctionVariables1);

                var chatFunctionVariables2 = new KernelArguments()
                {
                    ["problem"] = problem,
                    ["input"] = steps.ToString()
                };

                var result = await kernel.InvokeAsync(promptPlugin["chain_of_thought"], chatFunctionVariables2);

                var resultInt = int.Parse(result.ToString());

                results.Add(resultInt);
            }

            var mostCommonResult = results.GroupBy(x => x)
                                          .OrderByDescending(x => x.Count())
                                          .First()
                                          .Key;

            Console.WriteLine("Responses: ");

            foreach (var result in results)
            {
                Console.Write($"{result}, ");
            }

            Console.WriteLine($"\nFinal answer: {mostCommonResult}");
        }
    }

}