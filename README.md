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

You can use one of two ways to create tags, using the Blazorous component:

```
<Blazorous TagName="div" css="@css1">this is a nice box.</Blazorous>

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

You can see more examples in the sample project.

