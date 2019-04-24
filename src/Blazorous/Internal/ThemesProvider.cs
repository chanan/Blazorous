using System.Collections.Generic;

namespace Blazorous.Internal
{
    internal class ThemesProvider : IThemes
    {
        private ICssCreator _cssCreator;
        private readonly IDictionary<string, Theme> _themes = new Dictionary<string, Theme>();

        internal ThemesProvider(ICssCreator cssCreator)
        {
            _cssCreator = cssCreator;
            EmptyTheme = new Theme(cssCreator, "Empty");
        }

        IRelaxedDictionary<string, ITheme> IThemes.Themes
        {
            get
            {
                IRelaxedDictionary<string, ITheme> result = new RelaxedDictionary<string, ITheme>(EmptyTheme);
                foreach (var key in _themes.Keys) result.Add(key, _themes[key]);
                return result;
            }
        }

        public ITheme Current { get; set; }

        public ITheme EmptyTheme { get; }

        public ITheme CreateTheme(string name)
        {
            var theme = new Theme(_cssCreator, name);
            _themes.Add(name, theme);
            return theme;
        }

        public void SetCurrentToEmpty()
        {
            Current = EmptyTheme;
        }
    }
}
