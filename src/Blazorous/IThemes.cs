namespace Blazorous
{
    public interface IThemes
    {
        ITheme CreateTheme(string name);
        IRelaxedDictionary<string, ITheme> Themes { get; }
        ITheme Current { get; set; }
        void SetCurrentToEmpty();
        ITheme EmptyTheme { get; }
    }
}
