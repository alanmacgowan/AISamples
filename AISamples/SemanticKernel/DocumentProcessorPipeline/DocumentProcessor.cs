using Microsoft.SemanticKernel;
using Plugins.ProposalChecker;

namespace AISamples.SemanticKernel.DocumentProcessorPipeline;

public static class DocumentProcessor
{

    public static async Task RunSample()
    {

        var (apiKey, orgId) = ApplicationSettings.LoadFromEnvironment();

        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion("gpt-4", apiKey, orgId);
       // builder.Plugins.AddFromPromptDirectory("../../../plugins/ProposalChecker");
        builder.Plugins.AddFromPromptDirectory(ApplicationSettings.GetPluginsDirectory("DocumentProcessorPipeline", "ProposalChecker"));
        builder.Plugins.AddFromType<Helpers>();
        builder.Plugins.AddFromType<ParseWordDocument>();
        builder.Plugins.AddFromType<CheckSpreadsheet>();
        var kernel = builder.Build();

        KernelFunction processFolder = kernel.Plugins["Helpers"]["ProcessProposalFolder"];
        KernelFunction checkTabs = kernel.Plugins["CheckSpreadsheet"]["CheckTabs"];
        KernelFunction checkCells = kernel.Plugins["CheckSpreadsheet"]["CheckCells"];
        KernelFunction checkValues = kernel.Plugins["CheckSpreadsheet"]["CheckValues"];
        KernelFunction extractTeam = kernel.Plugins["ParseWordDocument"]["ExtractTeam"];
        KernelFunction checkTeam = kernel.Plugins["ProposalChecker"]["CheckTeam"];
        KernelFunction extractExperience = kernel.Plugins["ParseWordDocument"]["ExtractExperience"];
        KernelFunction checkExperience = kernel.Plugins["ProposalChecker"]["CheckPreviousProject"];
        KernelFunction extractImplementation = kernel.Plugins["ParseWordDocument"]["ExtractImplementation"];
        KernelFunction checkDates = kernel.Plugins["ProposalChecker"]["CheckDates"];

        KernelFunction pipeline = KernelFunctionCombinators.Pipe(new[] {
                                processFolder,
                                checkTabs,
                                checkCells,
                                checkValues,
                                extractTeam,
                                checkTeam,
                                extractExperience,
                                checkExperience,
                                extractImplementation,
                                checkDates
                            }, "pipeline");


        var proposals = Directory.GetDirectories("../../../SemanticKernel/DocumentProcessorPipeline/Data/proposals");

        foreach (var proposal in proposals)
        {
            string absolutePath = Path.GetFullPath(proposal);

            Console.WriteLine($"Processing {absolutePath}");
            KernelArguments context = new() { { "folderPath", absolutePath } };
            string? result = await pipeline.InvokeAsync<string>(kernel, context);
            Console.WriteLine(result);
            if (result == absolutePath)
            {
                Console.WriteLine("Success!");
            }

            Console.WriteLine();
        }

    }

}

