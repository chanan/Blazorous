using System.Collections.Generic;

namespace Blazorous.Internal
{
    internal class ThemesProvider : IThemes
    {
        internal static ThemesProvider Instance { get; private set; } = new ThemesProvider();
        
        private IDictionary<string, Theme> _themes = new Dictionary<string, Theme>();

        IDictionary<string, ITheme> IThemes.Themes
        {
            get
            {
                IDictionary<string, ITheme> result = new Dictionary<string, ITheme>();
                foreach (var key in _themes.Keys) result.Add(key, _themes[key]);
                return result;
            }
        }

        public ITheme Current { get; set; }

        public ITheme CreateTheme(string name)
        {
            var theme = new Theme(name);
            _themes.Add(name, theme);
            return theme;
        }
    }
}
