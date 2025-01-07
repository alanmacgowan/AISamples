using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Planning.Handlebars;

namespace AISamples.SemanticKernel.Planner;

#pragma warning disable SKEXP0060

public static class HomeAutomationPlanner
{

    public static async Task RunSample()
    {
        var (apiKey, orgId) = ApplicationSettings.LoadFromEnvironment();
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion("gpt-4", apiKey, orgId);
        builder.Plugins.AddFromType<HomeAutomation>();
        builder.Plugins.AddFromPromptDirectory(ApplicationSettings.GetPluginsDirectory("Planner", "MovieRecommender"));
        var kernel = builder.Build();

        void FulfillRequest(HandlebarsPlanner planner, string ask)
        {
            Console.WriteLine($"Fulfilling request: {ask}");
            var plan = planner.CreatePlanAsync(kernel, ask).Result;
            var result = plan.InvokeAsync(kernel, []).Result;
            Console.WriteLine("Request complete.");
        }

        var plannerOptions = new HandlebarsPlannerOptions()
        {
            ExecutionSettings = new OpenAIPromptExecutionSettings()
            {
                Temperature = 0.0,
                TopP = 0.1,
                MaxTokens = 4000
            },
            AllowLoops = true
        };

        var planner = new HandlebarsPlanner(plannerOptions);

        FulfillRequest(planner, "Turn on the lights in the kitchen");
        FulfillRequest(planner, "Open the windows of the bedroom, turn the lights off and put on Shawshank Redemption on the TV.");
        FulfillRequest(planner, "Close the garage door and turn off the lights in all rooms.");
        FulfillRequest(planner, "Turn off the lights in all rooms and play a movie in which Tom Cruise is a lawyer in the living room.");
    }

}

