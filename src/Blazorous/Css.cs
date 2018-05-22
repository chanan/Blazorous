using Blazorous.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorous
{
    public class Css : ICss, IRules
    {
        private IList<KeyValuePair<string, object>> _rules = new List<KeyValuePair<string, object>>();

        internal Css() { }
        public static ICss CreateNew()
        {
            return new Css();
        }

        public ICss AddRule(string name, string value)
        {
            return InternalAddRule(name, value);
        }

        public ICss AddRule(string name, int value)
        {
            return InternalAddRule(name, value);
        }

        public ICss AddRule(string name, float value)
        {
            return InternalAddRule(name, value);
        }

        public ICss AddRule(string name, double value)
        {
            return InternalAddRule(name, value);
        }

        public ICss AddDynamicRule(Action<IRules, IDictionary<string, object>> dynamicRule)
        {
            return InternalAddDynamicRule(dynamicRule);
        }

        public ICss AddClass(string name)
        {
            return InternalAddClass(name);
        }

        public ICss AddAnimation(string duration, Action<Animation> animation)
        {
            return InternalAddAnimation(duration, animation);
        }

        public ICss AddFontface(Action<IRules> fontFace)
        {
            return InternalAddFontface(fontFace);
        }

        public ICss AddSelector(string selector, Action<IRules> selectorRule)
        {
            return InternalAddSelector(selector, selectorRule);
        }

        string ICss.ToCss()
        {
            return ToCss(null, "false");
        }

        internal int Count {
            get => _rules.Count;
            private set => throw new InvalidOperationException(); 
        }

        public string ToCss(IDictionary<string, object> attributes, string debug)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < _rules.Count; i++)
            {
                var kvp = _rules[i];
                switch (kvp.Value)
                {
                    case string s:
                        sb.Append($"\"{kvp.Key}\": \"{s}\"");
                        break;
                    case int num:
                        sb.Append($"\"{kvp.Key}\": {num}");
                        break;
                    case DynamicRule dr:
                        var tempCss = new Css();
                        dr.Rule.Invoke(tempCss, attributes);
                        var cssRule = tempCss.ToCss(attributes, debug);
                        sb.Append(cssRule.Substring(1, cssRule.Length - 2));
                        break;
                    case Classname _:
                        break;
                    case AnimationStyle a:
                        var animationTemp = Animation.CreateNew();
                        a.Animation.Invoke(animationTemp);
                        var cssAnimation = animationTemp.ToKeyframes(attributes, debug);
                        sb.Append($"\"animation\": \"{cssAnimation} {a.Duration}\"");
                        break;
                    case FontFace f:
                        var tempFontfaceCss = new Css();
                        f.Fontface.Invoke(tempFontfaceCss);
                        var cssFontfaceRule = tempFontfaceCss.ToFontface(attributes, debug);
                        sb.Append($"\"font-family\": \"{cssFontfaceRule}\"");
                        break;
                    case SelectorRule sr:
                        var tempSelectorRuleCss = new Css();
                        sr.Rule.Invoke(tempSelectorRuleCss);
                        var cssSelectorRuleCss = tempSelectorRuleCss.ToCss(attributes, debug);
                        sb.Append($"\"{sr.Selector}\": {cssSelectorRuleCss}");
                        break;
                    default:
                        sb.Append($"\"{kvp.Key}\": \"{kvp.Value.ToString()}\"");
                        break;
                }
                if(i != _rules.Count - 1 && typeof(Classname) != kvp.Value.GetType() && typeof(Classname) != _rules[i + 1].Value.GetType()) sb.Append(",");
            }
            sb.Append("}");
            return sb.ToString();
        }

        public string ToClasses()
        {
            return ToClasses(null);
        }

        public string ToClasses(IDictionary<string, object> attributes)
        {
            var sb = new StringBuilder();
            foreach(var kvp in _rules)
            {
                switch(kvp.Value)
                {
                    case DynamicRule dr:
                        if (attributes == null) break;
                        var tempCss = new Css();
                        dr.Rule.Invoke(tempCss, attributes);
                        var classes = tempCss.ToClasses(attributes);
                        sb.Append(classes);
                        break;
                    case Classname c:
                        sb.Append(c.Name);
                        break;
                    default:
                        break;
                }
                sb.Append(" ");
            }
            return sb.ToString();
        }

        public string ToFontface()
        {
            return ToFontface(null, "false");
        }

        public string ToFontface(IDictionary<string, object> attributes, string debug)
        {
            var css = ToCss(attributes, debug);
            return BlazorousInterop.Fontface(css, debug);
        }

        IRules IRules.AddRule(string name, string value)
        {
            return InternalAddRule(name, value);
        }

        IRules IRules.AddRule(string name, int value)
        {
            return InternalAddRule(name, value);
        }

        IRules IRules.AddRule(string name, float value)
        {
            return InternalAddRule(name, value);
        }

        IRules IRules.AddRule(string name, double value)
        {
            return InternalAddRule(name, value);
        }

        IRules IRules.AddDynamicRule(Action<IRules, IDictionary<string, object>> dynamicRule)
        {
            return InternalAddDynamicRule(dynamicRule);
        }

        IRules IRules.AddClass(string name)
        {
            return InternalAddClass(name);
        }

        IRules IRules.AddSelector(string selector, Action<IRules> selectorRule)
        {
            return InternalAddSelector(selector, selectorRule);
        }

        private Css InternalAddRule(string name, string value)
        {
            _rules.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        private Css InternalAddRule(string name, int value)
        {
            _rules.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        private Css InternalAddRule(string name, float value)
        {
            _rules.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        private Css InternalAddRule(string name, double value)
        {
            _rules.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        private Css InternalAddDynamicRule(Action<IRules, IDictionary<string, object>> dynamicRule)
        {
            _rules.Add(new KeyValuePair<string, object>(string.Empty, new DynamicRule { Rule = dynamicRule }));
            return this;
        }

        private Css InternalAddClass(string name)
        {
            _rules.Add(new KeyValuePair<string, object>(name, new Classname { Name = name }));
            return this;
        }

        private Css InternalAddAnimation(string duration, Action<Animation> animation)
        {
            _rules.Add(new KeyValuePair<string, object>(String.Empty, new AnimationStyle { Duration = duration, Animation = animation }));
            return this;
        }

        private Css InternalAddFontface(Action<IRules> fontFace)
        {
            _rules.Add(new KeyValuePair<string, object>(String.Empty, new FontFace { Fontface = fontFace }));
            return this;
        }

        private Css InternalAddSelector(string selector, Action<IRules> selectorRule)
        {
            _rules.Add(new KeyValuePair<string, object>(selector, new SelectorRule { Selector = selector, Rule = selectorRule }));
            return this;
        }

        public ICss AddRules(params object[] list)
        {
            return InternalAddRules(list);
        }

        IRules IRules.AddRules(params object[] list)
        {
            return InternalAddRules(list);
        }

        private Css InternalAddRules(params object[] list)
        {
            for(int i = 0; i < list.Length; i += 2)
            {
                var str = list[i + 1].ToString();
                if (int.TryParse(str, out var num)) AddRule(list[i].ToString(), num);
                if (float.TryParse(str, out var f)) AddRule(list[i].ToString(), f);
                AddRule(list[i].ToString(), str);
            }
            return this;
        }
    }
}
