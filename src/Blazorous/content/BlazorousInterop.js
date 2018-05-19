Blazor.registerFunction('Blazorous.BlazorousInterop.Css', function (cssString, debug) {
    if(debug) console.log("Css String: %o", cssString);
    var cssJson = JSON.parse(cssString);
    if(debug) console.log("Css Json: %o", cssJson);
    var rule = Glamor.css(cssJson);
    return rule.toString();
});

Blazor.registerFunction('Blazorous.BlazorousInterop.keyframes', function (keyframes, debug) {
    if(debug) console.log("Keyframes: %o", keyframes);
    var cssKeyframes = JSON.parse(keyframes);
    if (debug) console.log("Css Keyframes: %o", cssKeyframes);
    var animation = Glamor.css.keyframes(cssKeyframes);
    return animation;
});