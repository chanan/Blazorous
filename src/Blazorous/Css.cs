using System;
using System.Collections.Generic;
using System.Text;

namespace Blazorous
{
    public class Css
    {
        private IList<KeyValuePair<string, object>> Rules { get; set; } = new List<KeyValuePair<string, object>>();
        private IList<object> Classes { get; set; } = new List<object>();
        public static Css CreateNew()
        {
            return new Css();
        }

        public Css AddRule(string name, string value)
        {
            Rules.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        public Css AddRule(string name, int value)
        {
            Rules.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        public Css AddRule(string name, float value)
        {
            Rules.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        public Css AddRule(string name, double value)
        {
            Rules.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        public Css AddDynamicRule(Action<Css, IDictionary<string, object>> dynamicRule)
        {
            Rules.Add(new KeyValuePair<string, object>(string.Empty, new DynamicRule { Rule = dynamicRule }));
            return this;
        }

        public Css AddClass(string name)
        {
            Rules.Add(new KeyValuePair<string, object>(name, new Classname { Name = name }));
            return this;
        }

        public Css AddAnimation(string duration, Action<Animation> animation)
        {
            Rules.Add(new KeyValuePair<string, object>(String.Empty, new AnimationStyle { Duration = duration, Animation = animation }));
            return this;
        }

        public Css AddFontface(Action<Css> fontFace)
        {
            Rules.Add(new KeyValuePair<string, object>(String.Empty, new FontFace { Fontface = fontFace }));
            return this;
        }

        public Css AddSelector(string selector, Action<Css> selectorRule)
        {
            Rules.Add(new KeyValuePair<string, object>(selector, new SelectorRule { Selector = selector, Rule = selectorRule }));
            return this;
        }

        public int Count {
            get => Rules.Count;
            private set => throw new InvalidOperationException(); 
        }

        public string ToCss()
        {
            return ToCss(null, "false");
        }

        public string ToCss(IDictionary<string, object> attributes, string debug)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < Rules.Count; i++)
            {
                var kvp = Rules[i];
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
                if(i != Rules.Count - 1 && typeof(Classname) != kvp.Value.GetType() && typeof(Classname) != Rules[i + 1].Value.GetType()) sb.Append(",");
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
            foreach(var kvp in Rules)
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

        private class DynamicRule
        {
            public Action<Css, IDictionary<string, object>> Rule { get; set; }
        }

        private class Classname
        {
            public string Name { get; set; }
        }

        private class AnimationStyle
        {
            public string Duration { get; set; }
            public Action<Animation> Animation { get; set; }
        }

        private class FontFace
        {
            public Action<Css> Fontface { get; set; }
        }

        private class SelectorRule
        {
            public string Selector { get; set; }
            public Action<Css> Rule { get; set; }
        }
    }
}
