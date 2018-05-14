using Microsoft.AspNetCore.Blazor.Browser.Interop;

namespace Blazorous
{
    public static class BlazorousInterop
    {
        public static string Css(string css)
        {
            return RegisteredFunction.Invoke<string>("Blazorous.BlazorousInterop.Css", css);
        }
    }
}
