
namespace Messaging.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class BsonElementNameAttribute : Attribute
    {
        public string ElementName { get; }

        public BsonElementNameAttribute(string elementName)
        {
            ElementName = elementName;
        }
    }
}
