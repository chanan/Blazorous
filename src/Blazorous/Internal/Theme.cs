using System;

namespace Blazorous.Internal
{
    internal class Theme : ITheme
    {
        internal Theme(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public IRelaxedDictionary<string, string> Variables { get; private set; } = new RelaxedDictionary<string, string>(String.Empty);
        public IRelaxedDictionary<string, ICss> Snippets { get; private set; } = new RelaxedDictionary<string, ICss>(Css.CreateNew());
        public override string ToString()
        {
            return Name;
        }
    }
}
