using System;
using System.Collections.Generic;

namespace Blazorous
{
    public interface ICss
    {
        ICss AddRule(string name, string value);
        ICss AddRule(string name, int value);
        ICss AddRule(string name, float value);
        ICss AddRule(string name, double value);
        ICss AddDynamicRule(Action<IRules, IDictionary<string, object>> dynamicRule);
        ICss AddClass(string name);
        ICss AddSelector(string selector, Action<IRules> selectorRule);
        ICss AddAnimation(string duration, Action<Animation> animation);
        ICss AddFontface(Action<IRules> fontFace);
        string ToCss();
        ICss AddRules(params object[] list);
        ICss AddMixin(string mixin);
    }
}
