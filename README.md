# Blazorous

*Maintainable CSS with Blazor*

## Install

Using Package Manager:
```
Install-Package Blazorous -Version 0.0.1
```

or dotnet CLI:
```
dotnet add package Blazorous --version 0.0.1
```

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
  Css cssCSharp = Css.CreateNew().AddRule("color", "red");
}
```

Or using the Blazorous Css() function directly with any component:

```
<div class="@classCSharp">this is a nice box.</div>

@functions {
  string classCSharp = Blazorous.BlazorousInterop.Css(Css.CreateNew().AddRule("color", "red").ToCss());
}
```

You can see more examples in the [docs](https://chanan.github.io/Blazorous/).