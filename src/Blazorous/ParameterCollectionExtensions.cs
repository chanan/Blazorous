using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;

namespace Blazorous
{
    internal static class ParameterCollectionExtensions
    {
        public static List<object> GetParameterList(this ParameterCollection parameters, string parameterName)
        {
            var list = new List<object>();
            foreach(var param in parameters)
            {
                if(param.Name.ToLower() == parameterName.ToLower())
                {
                    list.Add(param.Value);
                }
            }
            return list;
        }

        public static void Log(this ParameterCollection parameters)
        {
            foreach (var param in parameters)
            {
                Console.WriteLine("Param: {0} Value: {1}", param.Name, param.Value.ToString());
            }
        }
    }
}
