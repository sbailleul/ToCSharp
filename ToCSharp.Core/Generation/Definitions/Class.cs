using System.Collections.Immutable;
using System.Text;

namespace ToCSharp.Core.Generation.Definitions;

/// <summary>
///     Definition of an immutable class
/// </summary>
/// <param name="Name">Class name e.g : User</param>
/// <param name="Properties">Set of properties</param>
public record Class(string Name, IImmutableSet<Property>? Properties = null)
{
    /// <summary>
    ///     Create c# representation of the class
    /// </summary>
    /// <returns>The c# representation</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var property in Properties ?? Enumerable.Empty<Property>()) sb.AppendLine($"\t{property.ToString()}");

        return $@"
public class {Name}{{
{sb.ToString()}
}}";
    }
}