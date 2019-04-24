namespace Blazorous.Internal
{
    internal class CssCreator : ICssCreator
    {
        private readonly IBlazorousInterop _blazorousInterop;

        public CssCreator(IBlazorousInterop blazorousInterop)
        {
            _blazorousInterop = blazorousInterop;
        }

        internal IThemes ThemeProvider { get; set; }

        public ICss CreateNew()
        {
            return new Css(_blazorousInterop, ThemeProvider, this);
        }
    }
}
