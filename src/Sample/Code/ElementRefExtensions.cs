using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Sample.Code
{
    public static class ElementRefExtensions
    {
        public static async Task AlertValue(this ElementRef elementRef)
        {
            await JSRuntime.Current.InvokeAsync<bool>("blazorousSample.alertValue", elementRef);
        }
    }
}
