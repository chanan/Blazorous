using Microsoft.AspNetCore.Blazor.Components;
using System.Collections.Generic;

namespace Blazorous
{
    internal static class ParameterCollectionExtensions
    {
        public static List<object> GetParameterList(this ParameterCollection parameters, string ParameterName)
        {
            var list = new List<object>();
            foreach(var param in parameters)
            {
                if(param.Name.ToLower() == ParameterName.ToLower())
                {
                    list.Add(param.Value);
                }
            }
            return list;
        }
    }
}
