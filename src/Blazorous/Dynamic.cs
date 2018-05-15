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
            //parameters.Log();
            var css = parameters.GetParameterList("css");
            _attributesToRender = (IDictionary<string, object>)parameters.ToDictionary();
            _attributesToRender.Remove("css");
            _childContent = GetAndRemove<RenderFragment>(_attributesToRender, RenderTreeBuilder.ChildContent);
            _classname = GetAndRemove<string>(_attributesToRender, "class");

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

            var glamorClasses = GetCssFromProps(css, _attributesToRender);
            foreach (var classname in glamorClasses)
            {
                _classname = _classname != null ? $"{_classname} {classname}" : classname;
            }


            _renderHandle.Render(Render);
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

        private static List<string> GetCssFromProps(List<object> css, IDictionary<string, object> attributesToRender)
        {
            var list = new List<string>();
            foreach(var item in css)
            {
                switch(item)
                {
                    case string s:
                        list.Add(BlazorousInterop.Css(s));
                        break;
                    case Css c:
                        list.Add(BlazorousInterop.Css(c.ToCss(attributesToRender)));
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
    }
}
