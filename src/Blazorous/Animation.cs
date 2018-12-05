using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazorous
{
    public class Animation : IAnimation
    {
        private IList<KeyValuePair<string, Action<Css>>> Animations { get; set; } = new List<KeyValuePair<string, Action<Css>>>();

        internal Animation() { }
        public static IAnimation CreateNew()
        {
            return new Animation();
        }

        public IAnimation AddKeyframe(string keyframe, Action<ICss> css)
        {
            if(keyframe == "to" || keyframe == "from" || IsPercent(keyframe))
            {
                Animations.Add(new KeyValuePair<string, Action<Css>>(keyframe, css));
                return this;
            }
            throw new InvalidOperationException("keyframe must be 0% - 100%, \"from\", or \"to\"");
        }

        private bool IsPercent(string keyframe)
        {
            if(keyframe.EndsWith("%") && int.TryParse(keyframe.Substring(0, keyframe.Length - 1), out var i))
            {
                if (i >= 0 && i <= 100) return true;
            }
            return false;
        }

        internal int Count
        {
            get => Animations.Count;
            private set => throw new InvalidOperationException();
        }

        internal async Task<string> ToKeyframes()
        {
            return await ToKeyframes(null, "false");
        }

        internal async Task<string> ToKeyframes(IDictionary<string, object> attributes, string debug)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < Animations.Count; i++)
            {
                var kvp = Animations[i];
                sb.Append($"\"{kvp.Key}\":");
                var tempCss = new Css();
                kvp.Value.Invoke(tempCss);
                var cssRule = await tempCss.ToCss(attributes, debug);
                sb.Append(cssRule);
                if (i != Animations.Count - 1) sb.Append(",");
            }
            sb.Append("}");
            return await BlazorousInterop.Keyframes(sb.ToString(), debug);
        }
    }
}
