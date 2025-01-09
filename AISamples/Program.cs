using AISamples.Dotnet.SemanticKernel.ChainOfThought;
using AISamples.Dotnet.SemanticKernel.DocumentProcessorPipeline;
using AISamples.Dotnet.SemanticKernel.Planner;
using AISamples.Dotnet.SemanticKernel.Memory;

Console.WriteLine("================Start Running AI Sample================");

//await ChainOfThought.RunSample();
//await DocumentProcessor.RunSample();
//await HomeAutomationPlanner.RunSample();
await ChatWithMemory.RunSample();

Console.WriteLine("================Finish Running AI Sample================");
