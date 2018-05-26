using System;
using System.Collections.Generic;
using System.Text;
using Blazorous.Internal;

namespace Blazorous
{
    public class Css : ICss, IRules
    {
        private readonly IList<KeyValuePair<string, object>> _rules = new List<KeyValuePair<string, object>>();

        internal Css()
        {
        }

        internal int Count
        {
            get => _rules.Count;
            private set => throw new InvalidOperationException();
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

        public ICss AddRules(params object[] list)
        {
            return InternalAddRules(list);
        }

        public ICss AddMixin(string mixin)
        {
            return InternalAddMixin(mixin);
        }

        ICss ICss.ApplyThemeSnippet(string snippetName)
        {
            return InternalApplyThemeSnippet(snippetName);
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

        IRules IRules.AddRules(params object[] list)
        {
            return InternalAddRules(list);
        }

        IRules IRules.AddMixin(string mixin)
        {
            return InternalAddMixin(mixin);
        }

        IRules IRules.ApplyThemeSnippet(string snippetName)
        {
            return InternalApplyThemeSnippet(snippetName);
        }

        public static ICss CreateNew()
        {
            return new Css();
        }

        public string ToCss(IDictionary<string, object> attributes, string debug)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            for (var i = 0; i < _rules.Count; i++)
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
                        var animationTemp = new Animation();
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
                    case Mixin m:
                        var temp = BlazorousInterop.PolishedMixin(m.Rule, debug);
                        sb.Append(temp.Substring(1, temp.Length - 2));
                        break;
                    case ThemeSnippet ts:
                        var theme = ThemesProvider.Instance.Current;
                        if (theme != null)
                        {
                            var snippet = (Css)theme.Snippets[ts.SnippetName];
                            var cssSnippet = snippet.ToCss(attributes, debug);
                            sb.Append(cssSnippet.Substring(1, cssSnippet.Length - 2));
                        }
                        break;
                    default:
                        sb.Append($"\"{kvp.Key}\": \"{kvp.Value}\"");
                        break;
                }

                if (i != _rules.Count - 1 && typeof(Classname) != kvp.Value.GetType() &&
                    typeof(Classname) != _rules[i + 1].Value.GetType()) sb.Append(",");
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
            foreach (var kvp in _rules)
            {
                switch (kvp.Value)
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
            _rules.Add(new KeyValuePair<string, object>(string.Empty, new DynamicRule {Rule = dynamicRule}));
            return this;
        }

        private Css InternalAddClass(string name)
        {
            _rules.Add(new KeyValuePair<string, object>(name, new Classname {Name = name}));
            return this;
        }

        private Css InternalAddAnimation(string duration, Action<Animation> animation)
        {
            _rules.Add(new KeyValuePair<string, object>(string.Empty,
                new AnimationStyle {Duration = duration, Animation = animation}));
            return this;
        }

        private Css InternalAddFontface(Action<IRules> fontFace)
        {
            _rules.Add(new KeyValuePair<string, object>(string.Empty, new FontFace {Fontface = fontFace}));
            return this;
        }

        private Css InternalAddSelector(string selector, Action<IRules> selectorRule)
        {
            _rules.Add(new KeyValuePair<string, object>(selector,
                new SelectorRule {Selector = selector, Rule = selectorRule}));
            return this;
        }

        private Css InternalAddRules(params object[] list)
        {
            for (var i = 0; i < list.Length; i += 2)
            {
                var str = list[i + 1].ToString();
                if (int.TryParse(str, out var num)) AddRule(list[i].ToString(), num);
                if (float.TryParse(str, out var f)) AddRule(list[i].ToString(), f);
                AddRule(list[i].ToString(), str);
            }

            return this;
        }

        public Css InternalAddMixin(string mixin)
        {
            _rules.Add(new KeyValuePair<string, object>(string.Empty, new Mixin {Rule = mixin}));
            return this;
        }

        public Css InternalApplyThemeSnippet(string snippetName)
        {
            _rules.Add(new KeyValuePair<string, object>(string.Empty, new ThemeSnippet {SnippetName = snippetName}));
            return this;
        }
    }
}