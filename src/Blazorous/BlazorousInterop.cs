using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Blazorous
{
    public static class BlazorousInterop
    {
        public static Task<string> Css(string css)
        {
            return Css(css, "false");
        }

        internal static Task<string> Css(string css, string debug)
        {
            return JSRuntime.Current.InvokeAsync<string>("blazorous.css", css, Convert.ToBoolean(debug));
        }

        public static Task<string> Keyframes(string keyframes, string debug)
        {
            return JSRuntime.Current.InvokeAsync<string>("blazorous.keyframes", keyframes, Convert.ToBoolean(debug));
        }

        public static Task<string> Fontface(string fontface, string debug)
        {
            return JSRuntime.Current.InvokeAsync<string>("blazorous.fontface", fontface, Convert.ToBoolean(debug));
        }

        public static Task<string> PolishedMixin(string mixin, string debug)
        {
            return JSRuntime.Current.InvokeAsync<string>("blazorous.polishedMixin", mixin, debug);
        }
    }
}
