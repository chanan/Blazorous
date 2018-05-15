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

        public Css OpenSelector(string name)
        {
            Rules.Add(new KeyValuePair<string, object>(name, new OpenSelectorMarker()));
            return this;
        }

        public Css CloseSelector()
        {
            Rules.Add(new KeyValuePair<string, object>(string.Empty, new CloseSelectorMarker()));
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

        public string ToCss()
        {
            return ToCss(null);
        }

        public string ToCss(IDictionary<string, object> attributes)
        {
            var sb = new StringBuilder();
            bool inSelector = false;
            bool firstInSelector = true;
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
                    case OpenSelectorMarker _:
                        sb.Append($"\"{kvp.Key}\": {{");
                        inSelector = true;
                        firstInSelector = true;
                        break;
                    case CloseSelectorMarker _:
                        sb.Append("}");
                        inSelector = false;
                        break;
                    case DynamicRule dr:
                        inSelector = false;
                        if (attributes == null) break;
                        var tempCss = new Css();
                        dr.Rule.Invoke(tempCss, attributes);
                        var cssRule = tempCss.ToCss(attributes);
                        sb.Append(cssRule.Substring(1, cssRule.Length - 2));
                        break;
                    case Classname _:
                        break;
                    default:
                        sb.Append($"\"{kvp.Key}\": \"{kvp.Value.ToString()}\"");
                        break;
                }
                if(typeof(Classname) != kvp.Value.GetType())
                {
                    //TODO: Refactor the rules around commas
                    if (i == Rules.Count - 1 && inSelector) sb.Append("}");
                    if (i != Rules.Count - 1 && inSelector && !firstInSelector && typeof(CloseSelectorMarker) != Rules[i + 1].Value.GetType() && typeof(Classname) != Rules[i + 1].Value.GetType()) sb.Append(", ");
                    if (i != Rules.Count - 1 && !inSelector && typeof(CloseSelectorMarker) != Rules[i + 1].Value.GetType() && typeof(Classname) != Rules[i + 1].Value.GetType()) sb.Append(",");
                    firstInSelector = false;
                }
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
        private class OpenSelectorMarker { }

        private class CloseSelectorMarker { }

        private class DynamicRule
        {
            public Action<Css, IDictionary<string, object>> Rule { get; set; }
        }

        private class Classname
        {
            public string Name { get; set; }
        }
    }
}
