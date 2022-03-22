using System.Collections.Immutable;
using ToCSharp.Core.Generation.Definitions;

namespace ToCSharp.Core.Generation.Contracts;

/// <summary>
///     Parser contract
/// </summary>
public interface IParser
{
    /// <summary>
    ///     Generate class definitions from textReader content
    /// </summary>
    /// <param name="txtReader">Exchange format content</param>
    /// <returns>A set of class definitions</returns>
    public IImmutableSet<Class> Parse(TextReader txtReader);
}