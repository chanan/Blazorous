using System.Collections.Generic;

namespace Blazorous
{
    public interface ITheme
    {
        string Name { get; }
        IRelaxedDictionary<string, string> Variables { get; }
        IRelaxedDictionary<string, ICss> Snippets { get; }
    }
}
