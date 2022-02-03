using System.Reflection;
using Reports.Shrd.Exceptions;

namespace Reports.Shrd.Mappers.Impl
{
    public class SimpleMapper : IMapper
    {
        public T Map<T>(object o) where T : new()
        {
            T result = new T();
            foreach (var property in o.GetType().GetProperties())
            {
                PropertyInfo resultProperty = result.GetType().GetProperty(property.Name);
                if (resultProperty == null || resultProperty.PropertyType != property.PropertyType)
                {
                    throw new MappingException(
                        $"Object of type {o.GetType()} cannot be mapped into " +
                        $"object of type {typeof(T)}: property '{property.Name}' has no matching");
                }
                    
                resultProperty.SetValue(result, property.GetValue(o));
            }

            return result;
        }
    }
}