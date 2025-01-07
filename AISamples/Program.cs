// See https://aka.ms/new-console-template for more information
using AISamples.SemanticKernel.ChainOfThought;
using AISamples.SemanticKernel.DocumentProcessorPipeline;
using AISamples.SemanticKernel.Planner;

Console.WriteLine("Start Running AI Sample");

//await ChainOfThought.RunSample();
//await DocumentProcessor.RunSample();
await HomeAutomationPlanner.RunSample();

Console.WriteLine("Finish Running AI Sample");
