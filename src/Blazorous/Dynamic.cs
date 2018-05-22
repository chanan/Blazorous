using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;

namespace Blazorous
{
    public class Dynamic : IComponent, IHandleEvent
    {
        /// <summary>
        /// Gets or sets the name of the element to render.
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Gets or sets the attributes to render.
        /// </summary>
        public IReadOnlyDictionary<string, object> Attributes
        {
            // The property is only declared for intellisense. It's not used at runtime.
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the <see cref="Microsoft.AspNetCore.Blazor.ElementRef"/>.
        /// </summary>
        public ElementRef ElementRef { get; private set; }

        private RenderHandle _renderHandle;
        private IDictionary<string, object> _attributesToRender;
        private RenderFragment _childContent;
        private string _classname;
        

        /// <inheritdoc />
        public void Init(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        /// <inheritdoc />
        public void SetParameters(ParameterCollection parameters)
        {
            var css = parameters.GetParameterList("css");
            _attributesToRender = (IDictionary<string, object>)parameters.ToDictionary();
            _attributesToRender.Remove("css");
            _childContent = GetAndRemove<RenderFragment>(_attributesToRender, RenderTreeBuilder.ChildContent);
            _classname = GetAndRemove<string>(_attributesToRender, "class");
            var debug = GetAndRemove<string>(_attributesToRender, "debug");

            TagName = GetAndRemove<string>(_attributesToRender, nameof(TagName))
                ?? throw new InvalidOperationException($"No value was supplied for required parameter '{nameof(TagName)}'.");

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
                        _attributesToRender[kvp.Key] = existingValue.ToString()
                            + " " + kvp.Value.ToString();
                    }
                    else
                    {
                        _attributesToRender[kvp.Key] = kvp.Value;
                    }
                }
            }

            var addedClasses = GetClassesFromProps(css, _attributesToRender);
            foreach (var classname in addedClasses)
            {
                _classname = _classname != null ? $"{_classname} {classname}" : classname;
            }

            var glamorClasses = GetCssFromProps(css, _attributesToRender, debug);
            foreach (var classname in glamorClasses)
            {
                _classname = _classname != null ? $"{_classname} {classname}".Trim() : classname;
            }

            Css dynamicCss = CreateDynamicCssFromAttributes(_attributesToRender);

            if (dynamicCss.Count > 0)
            {
                var dynamicClass = BlazorousInterop.Css(dynamicCss.ToCss(_attributesToRender, debug), debug);
                _classname = _classname != null ? $"{_classname} {dynamicClass}" : dynamicClass;
            }

            var pseudoCss = CreatePseudoCssFromAttributes(_attributesToRender, debug);

            foreach(var str in pseudoCss)
            {
                _classname = _classname != null ? $"{_classname} {str}".Trim() : str;
            }

            _renderHandle.Render(Render);
        }

        private List<string> CreatePseudoCssFromAttributes(IDictionary<string, object> attributesToRender, string debug)
        {
            var list = new List<string>();
            foreach (var pseudo in _pseudoProps)
            {
                if (attributesToRender.TryGetValue(pseudo, out var value))
                {
                    switch (value)
                    {
                        case string s:
                            list.Add(BlazorousInterop.Css($"{{\":{pseudo}\":{s}}}", debug));
                            break;
                        case Css c:
                            var css = c.ToCss(attributesToRender, debug);
                            if (c.Count > 0) list.Add(BlazorousInterop.Css($"{{\":{pseudo}\":{css}}}", debug));
                            break;
                        default:
                            throw new InvalidOperationException($"Pseudo attribute {pseudo} must be string or of type Blazorous.Css");

                    }
                    attributesToRender.Remove(pseudo);
                }
            }
            return list;
        }

        private Css CreateDynamicCssFromAttributes(IDictionary<string, object> attributesToRender)
        {
            var css = new Css();
            foreach(var cssProp in _cssProps)
            {
                if (attributesToRender.TryGetValue(cssProp, out var value))
                {
                    css.AddRule(cssProp, value.ToString());
                    attributesToRender.Remove(cssProp);
                }
            }
            return css;
        }

        private List<string> GetClassesFromProps(List<object> css, IDictionary<string, object> attributesToRender)
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
                        throw new InvalidOperationException("css attribute muse be string or of type Blazorous.Css");

                }
            }
            return list;
        }

        private static List<string> GetCssFromProps(List<object> css, IDictionary<string, object> attributesToRender, string debug)
        {
            var list = new List<string>();
            foreach(var item in css)
            {
                switch(item)
                {
                    case string s:
                        list.Add(BlazorousInterop.Css(s, debug));
                        break;
                    case Css c:
                        if(c.Count > 0) list.Add(BlazorousInterop.Css(c.ToCss(attributesToRender, debug), debug).Trim());
                        break;
                    default:
                        throw new InvalidOperationException("css attribute muse be string or of type Blazorous.Css");

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

        private void Render(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, TagName);
            foreach (var kvp in _attributesToRender)
            {
                builder.AddAttribute(1, kvp.Key, kvp.Value);
            }
            if(_classname != null) builder.AddAttribute(2, "class", _classname);
            builder.AddElementReferenceCapture(3, capturedRef => { ElementRef = capturedRef; });
            builder.AddContent(4, _childContent);
            builder.CloseElement();
        }

        public void HandleEvent(EventHandlerInvoker handler, UIEventArgs args)
        {
            //Implementing IHandleEvent as a workaround for https://github.com/aspnet/Blazor/issues/656
            handler.Invoke(args);
        }

        private static readonly IList<string> _pseudoProps = new List<string>
        {
            "after",
            "before",
            "focus",
            "hover",
            "active",
            "visited"
        };

        private static readonly IList<string> _cssProps = new List<string>
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
