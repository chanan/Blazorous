using System;

namespace Blazorous.Internal
{
    internal class SelectorRule
    {
        public string Selector { get; set; }
        public Action<IRules> Rule { get; set; }
    }
}
