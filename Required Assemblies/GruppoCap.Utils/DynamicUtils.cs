using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Utils
{
    public static class DynamicUtils
    {
        // DYNAMIC HAS PROPERTY
        public static bool DynamicHasProperty(dynamic dynamicObject, string propertyName)
        {
            bool hasProperty = false;

            PropertyInfo[] properties = dynamicObject.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == propertyName)
                    return true;
            }

            return hasProperty;
        }

        // GET DYNAMIC PROPERTY VALUE
        public static dynamic GetDynamicPropertyValue(dynamic dynamicObject, string propertyName)
        {
            dynamic value = null;
            PropertyInfo propertyInfo = null;

            if (DynamicHasProperty(dynamicObject, propertyName))
            {
                propertyInfo = dynamicObject.GetType().GetProperty(propertyName);
                value = propertyInfo.GetValue(dynamicObject, null);
            }

            return value;
        }
    }
}
