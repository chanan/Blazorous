using System.Collections.Generic;

namespace Blazorous
{
    public interface ITheme
    {
        string Name { get; }
        IDictionary<string, string> Variables { get; }
    }
}
