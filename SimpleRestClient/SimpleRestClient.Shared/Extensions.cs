using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple
{
    public static class Extensions
    {
        public static bool HasProperty<T>(this object obj, string name, Action<T> result)
        {
            Type objType = obj.GetType();

            if (objType.GetProperty(name) != null)
            {
                var prop = objType.GetProperty(name);

                var value = prop.GetValue(obj,null);

                if (value is T)
                {
                    result((T)value);
                }
                else
                {
                    throw new InvalidCastException(string.Format("The property does not match the specified type '{0}'", prop.PropertyType));
                }
                return true;
            }

            return false;
        }

        
    }
}
