using System;
using Microsoft.AspNetCore.Blazor.Browser.Interop;

namespace Blazorous
{
    public class ExampleJsInterop
    {
        public static string Prompt(string message)
        {
            return RegisteredFunction.Invoke<string>(
                "Blazorous.ExampleJsInterop.Prompt",
                message);
        }
    }
}
