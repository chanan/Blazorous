using Blazorous.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazorous
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorous(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IBlazorousInterop, BlazorousInterop>();
            serviceCollection.AddSingleton<IAnimationCreator, AnimationCreator>();
            serviceCollection.AddSingleton<ICssCreator, CssCreator>();
            return serviceCollection;
        }

        public static IServiceCollection DefineBlazorousThemes(this IServiceCollection serviceCollection, Action<IThemes, ICssCreator> themes)
        {
            var sp = serviceCollection.BuildServiceProvider();
            var cssCreator = (CssCreator)sp.GetRequiredService<ICssCreator>();

            IThemes themesProvider = new ThemesProvider(cssCreator);
            cssCreator.ThemeProvider = themesProvider;
            serviceCollection.AddSingleton<IThemes>(themesProvider);
            themes(themesProvider, cssCreator);
            return serviceCollection;
        }
    }
}
