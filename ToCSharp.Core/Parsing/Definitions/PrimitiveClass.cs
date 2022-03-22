using System.Collections.Immutable;
using ToCSharp.Core.Extensions;

namespace ToCSharp.Core.Parsing.Definitions;

internal record PrimitiveClass(string Name, ISet<Property>? Properties = null) : Class(Name, Properties)
{
    public override Generation.Definitions.Class ToClass()
    {
        return new(Name.LowerFirst(), Properties
            .Select(p => p.ToProperty())
            .ToImmutableHashSet()
        );
    }
}