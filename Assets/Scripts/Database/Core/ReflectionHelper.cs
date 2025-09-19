using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ORMish
{
    public static class ReflectionHelper
    {
        public static List<Type> GetDerivedTypes(Type openGenericType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => IsSubclassOfRawGeneric(openGenericType, t))
                .ToList();
        }

        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                    return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }

}