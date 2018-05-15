Blazor.registerFunction('Blazorous.BlazorousInterop.Css', function (cssString) {
    //console.log("cssString: %o", cssString);
    var cssJson = JSON.parse(cssString);
    //console.log("cssJson: %o", cssJson);
    var rule = Glamor.css(cssJson);
    return rule.toString();
});