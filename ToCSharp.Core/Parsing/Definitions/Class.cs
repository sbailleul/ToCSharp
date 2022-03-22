using System.Collections.Immutable;
using ToCSharp.Core.Extensions;

namespace ToCSharp.Core.Parsing.Definitions;

internal record Class(string Name, ISet<Property>? Properties = null)
{
    public string Name { get; } = Name.UpperFirst();
    public ISet<Property> Properties { get; init; } = Properties ?? new HashSet<Property>();

    public virtual Generation.Definitions.Class ToClass()
    {
        return new(Name, Properties
            .Select(p => p.ToProperty())
            .ToImmutableHashSet()
        );
    }
}