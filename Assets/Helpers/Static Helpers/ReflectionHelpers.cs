using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Game
{
    public static class ReflectionHelpers
    {
        public static List<Type> GetImplementationsOf<TInterface>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly =>
                {
                    // Prevent issues with dynamic assemblies
                    try { return assembly.GetTypes(); }
                    catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null); }
                })
                .Where(type =>
                    typeof(TInterface).IsAssignableFrom(type) && // Implements the interface
                    !type.IsInterface &&
                    !type.IsAbstract)
                .ToList();
        }

        public static List<TypeNamePair> X<TInterface>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly =>
            {
            try { return assembly.GetTypes(); }
            catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null); }
            })
            .Where(t => typeof(TInterface).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .Select(t => new TypeNamePair(t))
            .ToList();
        }
    }

    public class TypeNamePair
    {
        public Type Type;
        public string Name;

        public TypeNamePair(Type type)
        {
            Type = type;
            Name = type.Name; // Or FullName
        }
    }
}