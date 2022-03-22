using CommandLine;
using ToCSharp.Core.Generation;
using ToCSharp.Core.Parsing;

namespace ToCSharp.CLI.CommandLine;

public static class ArgumentsProcessor
{
    public static Task ProcessAsync(string[] args)
    {
        return Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(RunAsync);
    }

    private static async Task RunAsync(Options options)
    {
        var parser = ExtensionMatcher.GetParser(options.File);
        var generator =
            new CSharpGenerator(parser, options.OutputDirectory ?? Directory.GetCurrentDirectory());
        await generator.WriteDefinitionsAsync(File.OpenText(options.File));
    }
}