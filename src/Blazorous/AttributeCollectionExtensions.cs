using System;
using System.Collections.Generic;

namespace Blazorous
{
    public static class AttributeCollectionExtensions
    {
        public static bool GetBooleanAttribute(this IDictionary<string, object> attributes, string attributeName)
        {
            return GetBooleanAttribute(attributes, attributeName, default);
        }

        public static bool GetBooleanAttribute(this IDictionary<string, object> attributes, string attributeName, bool defaultBool)
        {
            return attributes.ContainsKey(attributeName) ? Convert.ToBoolean(attributes[attributeName]) : defaultBool;
        }

        public static string GetStringAttribute(this IDictionary<string, object> attributes, string attributeName)
        {
            return GetStringAttribute(attributes, attributeName, default);
        }

        public static string GetStringAttribute(this IDictionary<string, object> attributes, string attributeName, string defaultString)
        {
            return attributes.ContainsKey(attributeName) ? attributes[attributeName].ToString() : defaultString;
        }

        public static int GetIntegerAttribute(this IDictionary<string, object> attributes, string attributeName)
        {
            return GetIntegerAttribute(attributes, attributeName, default);
        }

        public static int GetIntegerAttribute(this IDictionary<string, object> attributes, string attributeName, int defaultInt)
        {
            return attributes.ContainsKey(attributeName) ? Convert.ToInt32(attributes[attributeName]) : defaultInt;
        }

        public static bool TryGetValue<T>(this IDictionary<string, object> attributes, string attributeName, out T result)
        {
            return TryGetValue<T>(attributes, attributeName, default, out result);
        }

        public static bool TryGetValue<T>(this IDictionary<string, object> attributes, string attributeName, T defaultValue, out T result)
        {
            if(attributes.ContainsKey(attributeName))
            {
                result = (T)attributes[attributeName];
                return true;
            }
            result = defaultValue;
            return false;
        }
    }
}
