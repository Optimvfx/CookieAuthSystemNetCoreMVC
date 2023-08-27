using System.Reflection;

namespace Common.Helpers;

public class TypeHelper
{
    private static IEnumerable<Assembly> _allAssemblies => AppDomain.CurrentDomain.GetAssemblies();

    public static IEnumerable<Type> GetAllImplementations(Type type, IEnumerable<Assembly>? assemblies = null)
    {
        assemblies = assemblies ?? _allAssemblies;
        
        return assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => type.IsAssignableFrom(t));
    }

    public static T CreateNew<T>()
        where T : new()
    {
         return (T)Activator.CreateInstance(typeof(T));
    }
}