// See https://aka.ms/new-console-template for more information
using AISamples.SemanticKernel.DocumentProcessorPipeline;

Console.WriteLine("Start Running AI Sample");

//await ChainOfThought.RunSample();
await DocumentProcessor.RunSample();

Console.WriteLine("Finish Running AI Sample");
