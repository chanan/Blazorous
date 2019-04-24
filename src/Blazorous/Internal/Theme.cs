using System;

namespace Blazorous.Internal
{
    internal class Theme : ITheme
    {
        internal Theme(ICssCreator cssCreator, string name)
        {
            Name = name;
            Snippets = new RelaxedDictionary<string, ICss>(cssCreator.CreateNew());
        }
        public string Name { get; private set; }
        public IRelaxedDictionary<string, string> Variables { get; private set; } = new RelaxedDictionary<string, string>(String.Empty);
        public IRelaxedDictionary<string, ICss> Snippets { get; private set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
