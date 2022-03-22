using System.Collections.Immutable;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToCSharp.Core.Extensions;
using ToCSharp.Core.Generation.Contracts;
using ToCSharp.Core.Parsing.Definitions;

namespace ToCSharp.Core.Parsing.Json;

public class JsonParser : IParser
{
    private const string RootName = "root";
    private readonly PrimitivesAggregate _primitivesAggregate = new();
    private HashSet<Class> _classes = new();

    public IImmutableSet<Generation.Definitions.Class> Parse(TextReader txtReader)
    {
        _classes = new HashSet<Class>();
        var parsingTarget = JsonConvert.DeserializeObject(txtReader.ReadToEnd());
        if (parsingTarget is null) throw new ArgumentException("Cannot deserialize content to json");

        var json = (JToken)parsingTarget;
        var root = SetRootElement(json);
        Generate(root);
        return _classes.Select(tmpClass => tmpClass.ToClass()).ToImmutableHashSet();
    }

    private static KeyValuePair<string, JToken?> SetRootElement(JToken json)
    {
        if (json is JArray) json = new JObject { { "Items", json } };
        return new KeyValuePair<string, JToken?>(RootName, json);
    }

    private Class? HandlePrimitive(JToken json)
    {
        return json.Type switch
        {
            JTokenType.Integer => _primitivesAggregate.IntClass,
            JTokenType.Boolean => _primitivesAggregate.BoolClass,
            JTokenType.Float => _primitivesAggregate.DecimalClass,
            JTokenType.String => _primitivesAggregate.StringClass,
            JTokenType.Date => _primitivesAggregate.DateTimeClass,
            _ => null
        };
    }

    private static bool IsNull(JToken? json)
    {
        return json is null || json.Type == JTokenType.Null;
    }


    private Class? Generate(KeyValuePair<string, JToken?> json)
    {
        if (IsNull(json.Value)) return null;

        var primitiveClass = HandlePrimitive(json.Value!);
        if (primitiveClass is not null) return primitiveClass;

        var className = json.Key;
        var workingClass = _classes.FirstOrDefault(c => c.Name == className.UpperFirst());
        if (workingClass is null)
        {
            workingClass = new Class(className);
            _classes.Add(workingClass);
        }

        if (json.Value is JObject jObject) AddProperties(jObject, workingClass);

        return workingClass;
    }

    private void AddProperties(JObject jObject, Class workingClass)
    {
        foreach (var property in jObject)
        {
            var propertyName = property.Key;
            if (property.Value is JArray jArray)
                AddCollectionProperty(jArray, propertyName, workingClass);
            else
                workingClass.Properties.Add(new Property(propertyName, Generate(property)));
        }
    }

    private void AddCollectionProperty(JArray list, string propertyName,
        Class workingClass)
    {
        var propertyClass = list.Select(item =>
        {
            var wrappedItem = new KeyValuePair<string, JToken?>(propertyName, item);
            return Generate(wrappedItem);
        }).ToList().First();
        workingClass.Properties.Add(new Property(propertyName, propertyClass, true));
    }
}