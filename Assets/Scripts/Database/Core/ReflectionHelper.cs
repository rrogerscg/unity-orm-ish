using System;
using System.Collections.Generic;
using System.Linq;

namespace ORMish
{
    public static class ReflectionHelper
    {
        public static List<Type> GetDerivedTypes(Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                           .SelectMany(a => a.GetTypes())
                            .Where(t => IsSubclassOfRawGeneric(baseType, t))
                            .ToList();
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            // Skip the type itself - we want subclasses, not the base class
            if (toCheck == generic ||
                (toCheck.IsGenericType && toCheck.GetGenericTypeDefinition() == generic))
            {
                return false;
            }

            while (toCheck != null && toCheck != typeof(object))
            {
                toCheck = toCheck.BaseType;
                if (toCheck != null)
                {
                    var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                    if (generic == cur)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

}