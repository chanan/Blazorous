using System;
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
    }
}
