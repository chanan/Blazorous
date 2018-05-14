using System.Collections.Generic;
using System.Text;

namespace Blazorous
{
    public class Css
    {
        public List<KeyValuePair<string, object>> Rules { get; set; } = new List<KeyValuePair<string, object>>();

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


        public string ToCss()
        {
            var sb = new StringBuilder();
            bool inSelector = false;
            bool firstInSelector = true;
            sb.Append("{");
            for(int i = 0; i < Rules.Count; i++)
            {
                var kvp = Rules[i];
                switch(kvp.Value)
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
                    default:
                        sb.Append($"\"{kvp.Key}\": \"{kvp.Value.ToString()}\"");
                        break;
                }
                if(i == Rules.Count - 1 && inSelector) sb.Append("}");
                if(i != Rules.Count - 1 && inSelector && !firstInSelector && typeof(CloseSelectorMarker) != Rules[i + 1].Value.GetType()) sb.Append(", ");
                if(i != Rules.Count - 1 && !inSelector) sb.Append(", ");
                firstInSelector = false;
            }
            sb.Append("}");
            return sb.ToString();
        }

        private class OpenSelectorMarker { }

        private class CloseSelectorMarker { }
    }
}
