using Blazorous;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;

namespace Sample
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
                services.DefineBlazorousThemes(themes =>
                {
                    var theme1 = themes.CreateTheme("Soothing Web Colors");
                    theme1.Variables.Add("primary", "#f23d5d");
                    theme1.Variables.Add("secondary", "#8c3d5d");

                    var theme2 = themes.CreateTheme("Harry Potter");
                    theme2.Variables.Add("primary", "#e10000");
                    theme2.Variables.Add("secondary", "#12159f");
                });
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
