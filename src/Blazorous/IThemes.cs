using System.Collections.Generic;

namespace Blazorous
{
    public interface IThemes
    {
        ITheme CreateTheme(string name);
        IDictionary<string, ITheme> Themes { get; }
        ITheme Current { get; set; }
        void SetCurrentToEmpty();
        ITheme EmptyTheme { get; }
    }
}
