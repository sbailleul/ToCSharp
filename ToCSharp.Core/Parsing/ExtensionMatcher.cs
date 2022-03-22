using ToCSharp.Core.Generation.Contracts;
using ToCSharp.Core.Parsing.Exceptions;
using ToCSharp.Core.Parsing.Json;
using ToCSharp.Core.Parsing.Yaml;

namespace ToCSharp.Core.Parsing;

public static class ExtensionMatcher
{
    private static readonly Dictionary<string, Func<IParser>> ParsersByExtensions = new()
    {
        { ".yaml", () => new YamlParser() },
        { ".json", () => new JsonParser() }
    };

    public static void SetParser(string extension, Func<IParser> parserFactory)
    {
        if (!extension.StartsWith('.')) extension = $".{extension}";
        ParsersByExtensions[extension] = parserFactory;
    }

    public static IParser GetParser(string fileName)
    {
        var fileExtension = Path.GetExtension(fileName);
        if (ParsersByExtensions.TryGetValue(fileExtension, out var parserFactory)) return parserFactory.Invoke();

        throw new FileExtensionNotHandled();
    }
}