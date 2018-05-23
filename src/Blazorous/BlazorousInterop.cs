using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Blazor.Browser.Interop;

namespace Blazorous
{
    public static class BlazorousInterop
    {
        public static string Css(string css)
        {
            return Css(css, "false");
        }

        internal static string Css(string css, string debug)
        {
            return RegisteredFunction.Invoke<string>("Blazorous.BlazorousInterop.Css", css, Convert.ToBoolean(debug));
        }

        public static string Keyframes(string keyframes, string debug)
        {
            return RegisteredFunction.Invoke<string>("Blazorous.BlazorousInterop.keyframes", keyframes, Convert.ToBoolean(debug));
        }

        public static string Fontface(string fontface, string debug)
        {
            return RegisteredFunction.Invoke<string>("Blazorous.BlazorousInterop.Fontface", fontface, Convert.ToBoolean(debug));
        }

        public static string Polished(string method, params object[] list)
        {
            var _list = new List<object>(list); //This line is needed see: https://github.com/aspnet/Blazor/issues/740
            return RegisteredFunction.Invoke<string>("Blazorous.BlazorousInterop.Polished", method, _list);
        }
    }
}
