using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazorous
{
    public class Dynamic : ComponentBase
    {
        [Parameter] private string TagName { get; set; }

        private Dictionary<string, object> _attributesToRender;
        private RenderFragment _childContent;
        private string _classname;
        private string _debug;
        private List<object> _css;
        [Parameter] private ElementRef ElementRef { get; set; }

        public IReadOnlyDictionary<string, object> Attributes
        {
            // The property is only declared for intellisense. It's not used at runtime.
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override Task SetParametersAsync(ParameterCollection parameters)
        {
            _css = parameters.GetParameterList("css");
            _attributesToRender = (Dictionary<string, object>)parameters.ToDictionary();
            _attributesToRender.Remove("css");
            TagName = GetAndRemove<string>(_attributesToRender, nameof(TagName))
                ?? throw new InvalidOperationException($"No value was supplied for required parameter '{nameof(TagName)}'.");
            _childContent = GetAndRemove<RenderFragment>(_attributesToRender, RenderTreeBuilder.ChildContent);
            _classname = GetAndRemove<string>(_attributesToRender, "class");
            _debug = GetAndRemove<string>(_attributesToRender, "debug");

            // Combine any explicitly-supplied attributes with the remaining parameters
            var attributesParam = GetAndRemove<IReadOnlyDictionary<string, object>>(_attributesToRender, nameof(Attributes));

            if (attributesParam != null)
            {
                foreach (var kvp in attributesParam)
                {
                    if (kvp.Value != null
                        && _attributesToRender.TryGetValue(kvp.Key, out var existingValue)
                        && existingValue != null)
                    {
                        _attributesToRender[kvp.Key] = existingValue
                            + " " + kvp.Value;
                    }
                    else
                    {
                        _attributesToRender[kvp.Key] = kvp.Value;
                    }
                }
            }

            return base.SetParametersAsync(ParameterCollection.Empty);
        }

        protected override Task OnParametersSetAsync()
        {
            return UpdateCss(_css, _debug, _classname, _attributesToRender);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            builder.OpenElement(0, TagName);
            foreach (var param in _attributesToRender)
            {
                builder.AddAttribute(1, param.Key, param.Value);
            }
            if (_classname != null) builder.AddAttribute(1, "class", _classname);
            builder.AddElementReferenceCapture(2, capturedRef => { ElementRef = capturedRef; });
            builder.AddContent(3, _childContent);
            builder.CloseElement();
        }

        private async Task UpdateCss(IReadOnlyCollection<object> css, string debug, string classname, IDictionary<string, object> attributesToRender)
        {
            var ret = classname;
            var addedClasses = GetClassesFromProps(css, attributesToRender);
            foreach (var cls in addedClasses)
            {
                ret = ret != null ? $"{ret} {cls}" : cls;
            }

            var glamorClasses = await GetCssFromProps(css, attributesToRender, debug);
            foreach (var cls in glamorClasses)
            {
                ret = ret != null ? $"{ret} {cls}".Trim() : cls;
            }

            var dynamicCss = CreateDynamicCssFromAttributes(attributesToRender);

            if (dynamicCss.Count > 0)
            {
                var dynamicClass = await BlazorousInterop.Css(await dynamicCss.ToCss(attributesToRender, debug), debug);
                ret = ret != null ? $"{ret} {dynamicClass}" : dynamicClass;
            }

            var pseudoCss = await CreatePseudoCssFromAttributes(attributesToRender, debug);

            foreach (var str in pseudoCss)
            {
                ret = ret != null ? $"{ret} {str}".Trim() : str;
            }

            _classname = ret;
        }

        private static async Task<List<string>> CreatePseudoCssFromAttributes(IDictionary<string, object> attributesToRender, string debug)
        {
            var list = new List<string>();
            foreach (var pseudo in PseudoProps)
            {
                if (attributesToRender.TryGetValue(pseudo, out var value))
                {
                    switch (value)
                    {
                        case string s:
                            list.Add(await BlazorousInterop.Css($"{{\":{pseudo}\":{s}}}", debug));
                            break;
                        case Css c:
                            var css = await c.ToCss(attributesToRender, debug);
                            if (c.Count > 0) list.Add(await BlazorousInterop.Css($"{{\":{pseudo}\":{css}}}", debug));
                            break;
                        default:
                            throw new InvalidOperationException($"Pseudo attribute {pseudo} must be string or of type Blazorous.Css");

                    }
                    attributesToRender.Remove(pseudo);
                }
            }
            return list;
        }

        private static Css CreateDynamicCssFromAttributes(IDictionary<string, object> attributesToRender)
        {
            var css = new Css();
            foreach (var cssProp in CssProps)
            {
                if (attributesToRender.TryGetValue(cssProp, out var value))
                {
                    css.AddRule(cssProp, value.ToString());
                    attributesToRender.Remove(cssProp);
                }
            }
            return css;
        }

        private static IEnumerable<string> GetClassesFromProps(IEnumerable<object> css, IDictionary<string, object> attributesToRender)
        {
            var list = new List<string>();
            foreach (var item in css)
            {
                switch (item)
                {
                    case string _:
                        break;
                    case Css c:
                        list.Add(c.ToClasses(attributesToRender));
                        break;
                    default:
                        throw new InvalidOperationException("css attribute must be string or of type Blazorous.Css");

                }
            }
            return list;
        }

        private static async Task<List<string>> GetCssFromProps(IEnumerable<object> css, IDictionary<string, object> attributesToRender, string debug)
        {
            var list = new List<string>();
            foreach (var item in css)
            {
                switch (item)
                {
                    case string s:
                        list.Add(await BlazorousInterop.Css(s, debug));
                        break;
                    case Css c:
                        if (c.Count > 0)
                        {
                            var cssVar = await BlazorousInterop.Css(await c.ToCss(attributesToRender, debug), debug);
                            list.Add(cssVar.Trim());
                        }
                        break;
                    default:
                        throw new InvalidOperationException("css attribute must be string or of type Blazorous.Css");

                }
            }
            return list;
        }

        private static T GetAndRemove<T>(IDictionary<string, object> values, string key)
        {
            if (values.TryGetValue(key, out var value))
            {
                values.Remove(key);
                return (T)value;
            }
            else
            {
                return default;
            }
        }

        private static readonly IList<string> PseudoProps = new List<string>
        {
            "after",
            "before",
            "focus",
            "hover",
            "active",
            "visited"
        };

        private static readonly IList<string> CssProps = new List<string>
        {
            "azimuth",
            "background",
            "background-attachment",
            "background-color",
            "background-image",
            "background-position",
            "background-repeat",
            "border",
            "border-bottom",
            "border-bottom-color",
            "border-bottom-style",
            "border-bottom-width",
            "border-collapse",
            "border-color",
            "border-left",
            "border-left-color",
            "border-left-style",
            "border-left-width",
            "border-right",
            "border-right-color",
            "border-right-style",
            "border-right-width",
            "border-spacing",
            "border-style",
            "border-top",
            "border-top-color",
            "border-top-style",
            "border-top-width",
            "border-width",
            "bottom",
            "box-sizing",
            "caption-side",
            "caret-color",
            "chains",
            "clear",
            "clip",
            "color",
            "content",
            "counter-increment",
            "counter-reset",
            "cue",
            "cue-after",
            "cue-before",
            "cursor",
            "direction",
            "display",
            "elevation",
            "empty-cells",
            "float",
            "flow",
            "font",
            "font-family",
            "font-size",
            "font-style",
            "font-variant",
            "font-weight",
            "grid",
            "grid-template",
            "grid-template-areas",
            "grid-template-columns",
            "grid-template-rows",
            "height",
            "left",
            "letter-spacing",
            "line-height",
            "list-style",
            "list-style-image",
            "list-style-position",
            "list-style-type",
            "margin",
            "margin-bottom",
            "margin-left",
            "margin-right",
            "margin-top",
            "max-height",
            "max-width",
            "min-height",
            "min-width",
            "opacity",
            "orphans",
            "outline",
            "outline-color",
            "outline-offset",
            "outline-style",
            "outline-width",
            "overflow",
            "padding",
            "padding-bottom",
            "padding-left",
            "padding-right",
            "padding-top",
            "page-break-after",
            "page-break-before",
            "page-break-inside",
            "pause",
            "pause-after",
            "pause-before",
            "pitch",
            "pitch-range",
            "play-during",
            "position",
            "presentation-level",
            "quotes",
            "resize",
            "richness",
            "right",
            "speak",
            "speak-header",
            "speak-numeral",
            "speak-punctuation",
            "speech-rate",
            "stress",
            "table-layout",
            "text-align",
            "text-decoration",
            "text-indent",
            "text-overflow",
            "text-transform",
            "top",
            "unicode-bidi",
            "vertical-align",
            "visibility",
            "voice-family",
            "volume",
            "white-space",
            "widows",
            "width",
            "word-spacing",
            "z-index"
        }; 
    }
}
