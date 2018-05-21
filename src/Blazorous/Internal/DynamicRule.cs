using System;
using System.Collections.Generic;

namespace Blazorous.Internal
{
    internal class DynamicRule
    {
        public Action<IRules, IDictionary<string, object>> Rule { get; set; }
    }
}
