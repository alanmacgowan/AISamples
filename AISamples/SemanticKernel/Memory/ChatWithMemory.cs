using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Core;

#pragma warning disable SKEXP0003, SKEXP0011, SKEXP0052, SKEXP0050

namespace AISamples.Dotnet.SemanticKernel.Memory;

public static class ChatWithMemory
{

    public static async Task RunSample()
    {
        var (apiKey, orgId) = ApplicationSettings.LoadFromEnvironment();
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion("gpt-4-turbo-preview", apiKey, orgId);
        var kernel = builder.Build();
        kernel.ImportPluginFromObject(new ConversationSummaryPlugin());

        const string prompt = @"
        Chat history:
        {{ConversationSummaryPlugin.SummarizeConversation $history}}

        User: {{$userInput}}
        ChatBot:";

        var executionSettings = new OpenAIPromptExecutionSettings { MaxTokens = 2000, Temperature = 0.8, };

        var chatFunction = kernel.CreateFunctionFromPrompt(prompt, executionSettings);
        var history = "";
        var arguments = new KernelArguments();
        arguments["history"] = history;

        var chatting = true;
        while (chatting)
        {

            Console.Write("User: ");
            var input = Console.ReadLine();

            if (input == null) { break; }
            input = input.Trim();

            if (input == "exit") { break; }

            arguments["userInput"] = input;
            var answer = await chatFunction.InvokeAsync(kernel, arguments);
            var result = $"\nUser: {input}\nAssistant: {answer}\n";

            history += result;
            arguments["history"] = history;

            Console.WriteLine(result);
        }

        Console.WriteLine("Goodbye!");
    }

}

