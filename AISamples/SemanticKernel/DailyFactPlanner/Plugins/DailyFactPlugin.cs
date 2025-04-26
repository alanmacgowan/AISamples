using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AISamples.Dotnet.SemanticKernel.DailyFactPlanner.Plugins;

public class DailyFactPlugin
{
    private const string TEMPLATE = @"Tell me an interesting fact from world 
        about an event that took place on {{$today}}.
        Be sure to mention the date in history for context.";

    private readonly KernelFunction _dailyFact;
    private readonly KernelFunction _currentDay;
    
    public DailyFactPlugin()
    {
        PromptExecutionSettings settings = new()
        {
            ExtensionData = new Dictionary<string, object>()
            {
                { "Temperature", 0.7 },
                { "MaxTokens", 250 }
            }

        };
        
        _dailyFact = KernelFunctionFactory.CreateFromPrompt(TEMPLATE, functionName: "GetDailyFactFunc", executionSettings: settings);
        
        _currentDay = KernelFunctionFactory.CreateFromMethod(() => DateTime.Now.ToString("MMMM dd"), "GetCurrentDay");
    }
    
    [KernelFunction, Description("Provides interesting historic facts for the current date.")]
    public async Task<string> GetDailyFact([Description("Current day"), Required] string today, Kernel kernel)
    {
        var result = await _dailyFact.InvokeAsync(kernel, new() { ["today"] = today }).ConfigureAwait(false);

        return result.ToString();
    }

    [KernelFunction, Description("Retrieves the current day.")]
    public async Task<string> GetCurrentDay(Kernel kernel)
    {
        var today = await _currentDay.InvokeAsync(kernel);

        return today.ToString();
    }
}
