namespace ToCSharp.Core.Parsing.Definitions;

internal record Property(string Name, Class? Type, bool IsCollection = false)
{
    public Generation.Definitions.Property ToProperty()
    {
        return new(Name, Type?.ToClass(), IsCollection);
    }
}