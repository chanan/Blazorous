using System.Collections.Generic;

namespace Blazorous.Internal
{
    internal class Theme : ITheme
    {

        internal Theme(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public IDictionary<string, string> Variables { get; private set; } = new Dictionary<string, string>();

        public override string ToString()
        {
            return Name;
        }
    }
}
