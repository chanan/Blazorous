using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Blazorous
{
    public class BlazorousInterop : IBlazorousInterop
    {
        private readonly IJSRuntime _jsRuntime;

        public BlazorousInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public Task<string> Css(string css)
        {
            return Css(css, "false");
        }

        public Task<string> Css(string css, string debug)
        {
            return _jsRuntime.InvokeAsync<string>("blazorous.css", css, Convert.ToBoolean(debug));
        }

        public Task<string> Keyframes(string keyframes, string debug)
        {
            return _jsRuntime.InvokeAsync<string>("blazorous.keyframes", keyframes, Convert.ToBoolean(debug));
        }

        public Task<string> Fontface(string fontface, string debug)
        {
            return _jsRuntime.InvokeAsync<string>("blazorous.fontface", fontface, Convert.ToBoolean(debug));
        }

        public Task<string> PolishedMixin(string mixin, string debug)
        {
            return _jsRuntime.InvokeAsync<string>("blazorous.polishedMixin", mixin, debug);
        }
    }
}
