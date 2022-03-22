namespace ToCSharp.Core.Parsing.Definitions;

internal class PrimitivesAggregate
{
    public Class IntClass { get; } = new PrimitiveClass("int");
    public Class DecimalClass { get; } = new PrimitiveClass("decimal");
    public Class StringClass { get; } = new PrimitiveClass("string");
    public Class DateTimeClass { get; } = new("DateTime");
    public Class BoolClass { get; } = new PrimitiveClass("bool");
}