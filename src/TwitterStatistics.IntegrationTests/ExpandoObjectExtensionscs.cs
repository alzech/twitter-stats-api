using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStatistics.IntegrationTests
{
    public static class ExpandoObjectExtensions
    {
        public static T? GetProperty<T>(this ExpandoObject obj, string propName)
        {
            var prop = obj.FirstOrDefault(x => x.Key == propName);
            if (prop.Value == null) return default(T);
            return (T)prop.Value;
        }

        public static bool HasKey(this ExpandoObject obj, string propName)
        {
            if (((IDictionary<string, object?>)obj).ContainsKey(propName))
                return false;
            return true;
        }
    }
}
