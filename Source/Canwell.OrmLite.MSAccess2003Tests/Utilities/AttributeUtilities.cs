using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Utilities
{
    public static class AttributeUtilities
    {
        public static string GetAlias<T>()
        {
            return typeof(T).GetCustomAttributes(true).OfType<AliasAttribute>().Select(customAttribute => customAttribute.Name).FirstOrDefault();
        }

        public static string GetAlias(Type entity)
        {
            return entity.GetCustomAttributes(true).OfType<AliasAttribute>().Select(customAttribute => customAttribute.Name).FirstOrDefault();
        }
    }
}
