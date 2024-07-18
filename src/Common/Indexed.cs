using System.Reflection;

namespace Common
{
    public abstract class Indexed
    {
        readonly Dictionary<string, PropertyInfo> properties = [];

        public Indexed()
        {
            //Cache property info to improve performance
            foreach (var prop in GetType().GetProperties())
            {
                properties[prop.Name] = prop;
            }
        }

        public T this<T>[string propertyName]
        {
            get
            {
                if (properties.TryGetValue(propertyName, out PropertyInfo propInfo))
                {
                    return propInfo.GetValue(this);
                }
                throw new ArgumentException($"Property '{propertyName}' was not found.");
            }
            set
            {
                if (properties.TryGetValue(propertyName, out PropertyInfo propInfo))
                {
                    propInfo.SetValue(this, value);
                }
                else
                {
                    throw new ArgumentException($"Property '{propertyName}' was not found.");
                }
            }
        }
    }
}
