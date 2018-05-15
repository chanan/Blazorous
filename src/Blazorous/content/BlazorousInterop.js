Blazor.registerFunction('Blazorous.BlazorousInterop.Css', function (cssString, debug) {
    if(debug) console.log("Css String: %o", cssString);
    var cssJson = JSON.parse(cssString);
    if(debug) console.log("Css Json: %o", cssJson);
    var rule = Glamor.css(cssJson);
    return rule.toString();
});