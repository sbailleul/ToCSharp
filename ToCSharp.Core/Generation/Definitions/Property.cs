using ToCSharp.Core.Extensions;

namespace ToCSharp.Core.Generation.Definitions;

/// <summary>
///     A class definition property
/// </summary>
/// <param name="IsCollection">Flag to handle collections in generic type</param>
public record Property(string Name, Class? Type, bool IsCollection)
{
    public override string ToString()
    {
        if (Type is null) return string.Empty;

        return $"public {GetTypeDef()} {Name.UpperFirst()} {{get;set;}}";
    }

    private string GetTypeDef()
    {
        if (IsCollection) return $"Collection<{Type!.Name}>";

        return Type!.Name;
    }
}