using System.ComponentModel;
using YamlDotNet.Core;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectGraphVisitors;
using YamlDotNet.Core.Events;

namespace SimpleCustomRoles;

public class CommentGatheringTypeInspector(ITypeInspector innerTypeDescriptor) : TypeInspectorSkeleton
{
    private readonly ITypeInspector innerTypeDescriptor = innerTypeDescriptor ?? throw new ArgumentNullException("innerTypeDescriptor");

    public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
    {
        return innerTypeDescriptor
            .GetProperties(type, container)
            .Select(d => new CommentsPropertyDescriptor(d));
    }

    private sealed class CommentsPropertyDescriptor(IPropertyDescriptor baseDescriptor) : IPropertyDescriptor
    {
        private readonly IPropertyDescriptor baseDescriptor = baseDescriptor;

        public string Name { get; set; } = baseDescriptor.Name;

        public Type Type { get { return baseDescriptor.Type; } }

        public Type TypeOverride
        {
            get { return baseDescriptor.TypeOverride; }
            set { baseDescriptor.TypeOverride = value; }
        }

        public int Order { get; set; }

        public ScalarStyle ScalarStyle
        {
            get { return baseDescriptor.ScalarStyle; }
            set { baseDescriptor.ScalarStyle = value; }
        }

        public bool CanWrite { get { return baseDescriptor.CanWrite; } }

        public void Write(object target, object value)
        {
            baseDescriptor.Write(target, value);
        }

        public T GetCustomAttribute<T>() where T : Attribute
        {
            return baseDescriptor.GetCustomAttribute<T>();
        }

        public IObjectDescriptor Read(object target)
        {
            var description = baseDescriptor.GetCustomAttribute<DescriptionAttribute>();
            return description != null
                ? new CommentsObjectDescriptor(baseDescriptor.Read(target), description.Description)
                : baseDescriptor.Read(target);
        }
    }
}
public sealed class CommentsObjectDescriptor(IObjectDescriptor innerDescriptor, string comment) : IObjectDescriptor
{
    private readonly IObjectDescriptor innerDescriptor = innerDescriptor;

    public string Comment { get; private set; } = comment;

    public object Value { get { return innerDescriptor.Value; } }
    public Type Type { get { return innerDescriptor.Type; } }
    public Type StaticType { get { return innerDescriptor.StaticType; } }
    public ScalarStyle ScalarStyle { get { return innerDescriptor.ScalarStyle; } }
}
public class CommentsObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor) : ChainedObjectGraphVisitor(nextVisitor)
{
    public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
    {
        if (value is CommentsObjectDescriptor commentsDescriptor && commentsDescriptor.Comment != null)
        {
            context.Emit(new Comment(commentsDescriptor.Comment, false));
        }

        return base.EnterMapping(key, value, context);
    }
}
