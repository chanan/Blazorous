using Blazorous.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazorous
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection DefineBlazorousThemes(this IServiceCollection serviceCollection, Action<IThemes> themes)
        {
            themes(ThemesProvider.Instance);

            serviceCollection.AddSingleton<IThemes>(ThemesProvider.Instance);
            return serviceCollection;
        }
    }
}
