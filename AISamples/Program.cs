using AISamples.Dotnet.SemanticKernel.ChainOfThought;
using AISamples.Dotnet.SemanticKernel.DocumentProcessorPipeline;
using AISamples.Dotnet.SemanticKernel.Planner;
using AISamples.Dotnet.SemanticKernel.Memory;
using AISamples.Dotnet.SemanticKernel.DailyFactPlanner;

Console.WriteLine("================Start Running AI Sample================");

//await ChainOfThought.RunSample();
//await DocumentProcessor.RunSample();
//await HomeAutomationPlanner.RunSample();
//await ChatWithMemory.RunSample();
await DailyFactPlanner.RunSample();

Console.WriteLine("================Finish Running AI Sample================");
