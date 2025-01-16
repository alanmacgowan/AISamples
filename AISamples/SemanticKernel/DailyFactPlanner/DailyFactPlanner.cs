using AISamples.Dotnet.SemanticKernel.DailyFactPlanner.Configuration;
using AISamples.Dotnet.SemanticKernel.DailyFactPlanner.Extensions;
using AISamples.Dotnet.SemanticKernel.DailyFactPlanner.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;

namespace AISamples.Dotnet.SemanticKernel.DailyFactPlanner;

#pragma warning disable SKEXP0060

public static class DailyFactPlanner
{

    public static async Task RunSample()
    {
        var (apiKey, orgId) = ApplicationSettings.LoadFromEnvironment();

        var config = Configuration.Configuration.ConfigureAppSettings();

        // Get Settings (all this is just so I don't have hard coded config settings here)
        //var openAiSettings = new OpenAIOptions();
        //config.GetSection(OpenAIOptions.OpenAI).Bind(openAiSettings);

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Information);

            builder.AddConfiguration(config);
            builder.AddConsole();
        });

        // Configure Semantic Kernel
        var builder = Kernel.CreateBuilder();

        builder.Services.AddSingleton(loggerFactory);

        builder.AddOpenAIChatCompletion("gpt-4", apiKey, orgId);

        //builder.AddChatCompletionService(openAiSettings);
        //builder.AddChatCompletionService(openAiSettings, ApiLoggingLevel.ResponseAndRequest); // use this line to see the JSON between SK and OpenAI


        builder.Plugins.AddFromType<DailyFactPlugin>();

        Kernel kernel = builder.Build();

        // TODO: CHALLENGE 1: does the AI respond accurately to this prompt? How to fix?
        var prompt = $"Tell me an interesting fact from world about an event " +
                    $"that took place on today's date. " +
                    $"Be sure to mention the date in history for context.";

        var planner = new HandlebarsPlanner(new HandlebarsPlannerOptions() { AllowLoops = true });

        var plan = await planner.GetOrCreatePlanAsync("SavedPlan.hbs", kernel, prompt);

        WriteLine($"\nPLAN: \n\n{plan}");

        var result = await plan.InvokeAsync(kernel);

        WriteLine($"\nRESPONSE: \n\n{result}");
    }

    static void WriteLine(string message)
    {
        Console.WriteLine("----------------------------------------------");

        Console.WriteLine(message);

        Console.WriteLine("----------------------------------------------");
    }

}

