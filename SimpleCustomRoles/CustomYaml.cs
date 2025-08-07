using LabApi.Loader.Features.Yaml.CustomConverters;
using Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SimpleCustomRoles;

public static class CustomYaml
{
    public static ISerializer Serializer { get; } = new SerializerBuilder()
        .WithEmissionPhaseObjectGraphVisitor((EmissionPhaseObjectGraphVisitorArgs visitor) => new CommentsObjectGraphVisitor(visitor.InnerVisitor))
        .WithTypeInspector((ITypeInspector typeInspector) => new CommentGatheringTypeInspector(typeInspector))
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .DisableAliases()
        .IgnoreFields()
        .WithTypeConverter(new CustomVectorConverter())
        .WithTypeConverter(new CustomColor32Converter())
        .WithTypeConverter(new CustomColorConverter())
        .Build();

    public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .IgnoreFields()
        .WithTypeConverter(new CustomVectorConverter())
        .WithTypeConverter(new CustomColor32Converter())
        .WithTypeConverter(new CustomColorConverter())
        .Build();
}
