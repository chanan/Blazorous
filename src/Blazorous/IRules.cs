using System;
using System.Collections.Generic;

namespace Blazorous
{
    public interface IRules
    {
        IRules AddRule(string name, string value);
        IRules AddRule(string name, int value);
        IRules AddRule(string name, float value);
        IRules AddRule(string name, double value);
        IRules AddDynamicRule(Action<IRules, IDictionary<string, object>> dynamicRule);
        IRules AddClass(string name);
        IRules AddSelector(string selector, Action<IRules> selectorRule);
    }
}
