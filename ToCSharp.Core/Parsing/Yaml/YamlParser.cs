using System.Collections.Immutable;
using ToCSharp.Core.Extensions;
using ToCSharp.Core.Generation.Contracts;
using ToCSharp.Core.Parsing.Definitions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ToCSharp.Core.Parsing.Yaml;

public class YamlParser : IParser
{
    private const string RootName = "root";
    private readonly PrimitivesAggregate _primitivesAggregate = new();
    private HashSet<Class> _classes = new();

    public IImmutableSet<Generation.Definitions.Class> Parse(TextReader txtReader)
    {
        _classes = new HashSet<Class>();
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance) // see height_in_inches in sample yml 
            .Build();
        var parsingTarget = deserializer.Deserialize(txtReader);
        if (parsingTarget is null) throw new ArgumentException("Cannot deserialize YAML");
        Generate(SetRootElement(parsingTarget));
        return _classes.Select(tmpClass => tmpClass.ToClass()).ToImmutableHashSet();
    }

    private static KeyValuePair<object, object?> SetRootElement(object parsingTarget)
    {
        if (parsingTarget is List<object> itemsList)
            parsingTarget = new Dictionary<object, object>
            {
                { "items", itemsList }
            };
        return new KeyValuePair<object, object?>("Root", parsingTarget);
    }

    private Class? HandlePrimitive(object value)
    {
        var txtValue = value.ToString();
        if (int.TryParse(txtValue, out var i)) return _primitivesAggregate.IntClass;
        if (bool.TryParse(txtValue, out var b)) return _primitivesAggregate.BoolClass;

        if (decimal.TryParse(txtValue, out var d)) return _primitivesAggregate.DecimalClass;

        if (DateTime.TryParse(txtValue, out var t)) return _primitivesAggregate.DateTimeClass;

        return Equals(txtValue, value) ? _primitivesAggregate.StringClass : null;
    }

    private Class? Generate(KeyValuePair<object, object?> classDef)
    {
        if (classDef.Value is null) return null;

        var primitiveClass = HandlePrimitive(classDef.Value);
        if (primitiveClass is not null) return primitiveClass;

        var className = classDef.Key.ToString();
        if (className is null) return null;

        var workingClass = _classes.FirstOrDefault(c => c.Name == className.UpperFirst());
        if (workingClass is null)
        {
            workingClass = new Class(className);
            _classes.Add(workingClass);
        }

        if (classDef.Value is Dictionary<object, object?> properties) AddProperties(properties, workingClass);

        return workingClass;
    }

    private void AddProperties(Dictionary<object, object?> properties, Class workingClass)
    {
        foreach (var property in properties)
        {
            var propertyName = property.Key.ToString();
            if (propertyName is null) break;

            if (property.Value is List<object> list)
                AddCollectionProperty(list, propertyName, workingClass);
            else
                workingClass.Properties.Add(new Property(propertyName, Generate(property)));
        }
    }

    private void AddCollectionProperty(IEnumerable<object> list, string propertyName,
        Class workingClass)
    {
        var propertyClass = list.Select(item =>
        {
            var wrappedItem = new KeyValuePair<object, object?>(propertyName, item);
            return Generate(wrappedItem);
        }).ToList().First();
        workingClass.Properties.Add(new Property(propertyName, propertyClass, true));
    }
}