using System.Xml.Linq;
using ToCSharp.CLI.CommandLine;
using ToCSharp.Core.Parsing;
using ToCSharp.Core.Parsing.Xml;

ExtensionMatcher.SetParser(".xml", () => new XmlParser());
await ArgumentsProcessor.ProcessAsync(args);