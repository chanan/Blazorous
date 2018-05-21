# Blazorous

_Maintainable CSS with Blazor_

## Install


[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Blazorous.svg)](https://www.nuget.org/packages/Blazorous/)


Add the tag helper to your page or `_ViewImports.cshtml`

```
@addTagHelper *,Blazorous
```

## Define your CSS inline

You can use one of two ways to create tags, using the Dynamic component:

```
<Dynamic TagName="div" css="@css1">this is a nice box.</Dynamic>

@functions {
  string css1 = "{ \"color\": \"red\" }";
}
```

Or using the Blazorous Css() function directly with any component:

```
<div class="@class1">this is a nice box.</div>

@functions {
  string class1 = Blazorous.BlazorousInterop.Css("{ \"color\": \"red\" }");
}
```

## Define your CSS inline using C# syntax

You can also define your styles using C# syntax:

```
<Dynamic TagName="div" css="@cssCSharp">this is a nice box.</Dynamic>

@functions {
  ICss cssCSharp = Css.CreateNew().AddRule("color", "red");
}
```

Or using the Blazorous Css() function directly with any component:

```
<div class="@classCSharp">this is a nice box.</div>

@functions {
  string classCSharp = Blazorous.BlazorousInterop.Css(Css.CreateNew().AddRule("color", "red").ToCss());
}
```

## Dynamic Rules

Dynamic rules allow you to define styles that change dynamically based on attributes on your Component. For example, if you define a Css attribute:

```
ICss coloredDiv = Css.CreateNew()
  .AddDynamicRule((css, attributes) =>
  {
    css.AddRule(<span style="color:#093">"color"</span>, attributes.GetStringAttribute(<span style="color:#093">"color"</span>, <span style="color:#093">"black"</span>));
  });
```

You can then use it wil different colors:

```
<Dynamic TagName="div" css="@coloredDiv" color="red">This &lt;div&gt; has an attribute: color="red"</Dynamic>

<Dynamic TagName="div" css="@coloredDiv" color="blue">This &lt;div&gt; has an attribute: color="blue"</Dynamic>
```

A more complete example is in the [Dynamic Rules](https://chanan.github.io/Blazorous/dynamic.html) section of the docs.

## Style Attributes

For many simple cases, you can define your CSS properties directly on the `Dynamic` tag:

```
<Dynamic TagName="div" background-color="red" color="white" font-size="50">This is awesome!</Dynamic>
```

## Animations

Css keyframe animation can be added via the Css object:

```
<Dynamic TagName="div" css="@css"></Dynamic>

@functions {
  ICss css = Css.CreateNew()
    .AddAnimation("0.2s infinite ease-in-out alternate", animation =>
    {
        animation
        .AddKeyframe("0%", css => css.AddRule("transform", "scale(0.1)").AddRule("opacity", 0))
        .AddKeyframe("60%", css => css.AddRule("transform", "scale(1.2)").AddRule("opacity", 1))
        .AddKeyframe("100%", css => css.AddRule("transform", "scale(1)"));
    }).AddRule("width", 50)
    .AddRule("height", 50)
    .AddRule("background-color", "red");
}

```

## Fonts

Fonts can be added to elements by calling the `AddFontFace`:

```
<Dynamic TagName="h1" css="@css">Font Faces</Dynamic>

@functions {
  ICss css = Css.CreateNew()
    .AddFontface(css =>
    {
      css.AddRule("fontFamily", "Indie Flower")
        .AddRule("fontStyle", "normal")
        .AddRule("fontWeight", 400)
        .AddRule("src", "local('Indie Flower'), local('IndieFlower'), url(https://fonts.gstatic.com/s/indieflower/v9/m8JVjfNVeKWVnh3QMuKkFcZVaUuH.woff2) format('woff2')")
        .AddRule("unicodeRange", "U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD");
    });
}
``` 

## Docs

You can see more examples in the [docs](https://chanan.github.io/Blazorous/).
