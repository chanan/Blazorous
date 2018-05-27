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
                    theme1.Variables.Add("font_color", "white");
                    theme1.Snippets.Add("heading", Css.CreateNew().AddRule("color", "#423d5d").AddFontface(css =>
                    {
                        css.AddRule("fontFamily", "Fira Sans")
                            .AddRule("fontStyle", "normal")
                            .AddRule("fontWeight", 400)
                            .AddRule("src", "local('Fira Sans Regular'), local('FiraSans-Regular'), url(https://fonts.gstatic.com/s/firasans/v8/va9E4kDNxMZdWfMOD5Vvl4jL.woff2) format('woff2')")
                            .AddRule("unicodeRange", "U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD");
                    }));

                    var theme2 = themes.CreateTheme("Harry Potter");
                    theme2.Variables.Add("primary", "#e10000");
                    theme2.Variables.Add("secondary", "#12159f");
                    theme2.Variables.Add("font_color", "white");
                    theme2.Snippets.Add("heading", Css.CreateNew().AddRules("color", "#008709").AddFontface(css =>
                    {
                        css.AddRule("fontFamily", "Butterfly Kids")
                            .AddRule("fontStyle", "normal")
                            .AddRule("fontWeight", 400)
                            .AddRule("src", "local('Butterfly Kids Regular'), local('ButterflyKids-Regular'), url(https://fonts.gstatic.com/s/butterflykids/v6/ll8lK2CWTjuqAsXDqlnIbMNs5R4dpRA.woff2) format('woff2')")
                            .AddRule("unicodeRange", "U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD");
                    }));
                });
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
