Blazor.registerFunction('Blazorous.BlazorousInterop.Css', function (cssString, debug) {
    if (debug) console.log("Css String: %o", cssString);
    var cssPolished = parsePolishedFunctions(cssString);
    if (debug) console.log("Css String Polished: ", cssPolished);
    var cssJson = JSON.parse(cssPolished);
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

Blazor.registerFunction('Blazorous.BlazorousInterop.Fontface', function (fontface, debug) {
    if (debug) console.log("fontface: %o", fontface);
    var cssFontface = JSON.parse(fontface);
    if (debug) console.log("cssFontface: %o", cssFontface);
    var family = Glamor.css.fontFace(cssFontface);
    return family;
});

Blazor.registerFunction('Blazorous.BlazorousInterop.Polished', function (method, args) {
    var x = polished[method].apply(this, args);
    console.log("x: ", x);
    return x;
});


//TODO: Instead of while(true) need the loop to walk forward
function parsePolishedFunctions(cssString) {
    var result = cssString;
    let found = false;
    let loopStart = 0;
    

    polishedFunctions.forEach(f => {
        loopStart = 0;
        while (true) {
            found = false;
            if (result.indexOf(f + "(", loopStart) !== -1) {
                var start = result.indexOf(f + "(", loopStart);
                loopStart = start;
                var end = getClosingParens(result, start);
                var methodWithParams = result.substring(start, end);
                console.log("methodWithParams: ", methodWithParams);
                var method = methodWithParams.substring(0, methodWithParams.indexOf("("));
                var params = methodWithParams.substring(methodWithParams.indexOf("(") + 1, methodWithParams.length - 1);
                console.log("method: ", method);
                console.log("params string: ", params);
                var paramsParsed = parsePolishedFunctions(params);
                console.log("paramsParsed: ", paramsParsed);
                var list = paramsParsed.split(",");
                var args = []
                list.forEach(a => args.push(normalizeArg(a)));
                console.log("args: %o", args);
                var polishedResult = polished[method].apply(this, args);
                loopStart += polishedResult.length;
                console.log(loopStart);
                console.log("polishedResult: ", polishedResult);
                result = result.replace(methodWithParams, polishedResult);
                found = true;
            }
            if (!found || loopStart > result.length) break;
        }
    });
    return result;
}

function getClosingParens(strIn, start) {
    var str = strIn.substring(start);
    var chars = str.split('');
    let i = 0;
    let count = 0;
    while (true) {
        var char = str.charAt(i);
        if (char === '(') count++;
        if (char === ')') {
            if (count === 1) break;
            count--;
        }
        i++;
        if (i > str.length) throw "Parenthesis were not balanced";
    }
    return start + i + 1;
}

function normalizeArg(arg) {
    var result = arg.trim();
    if (isNumeric(result)) result = parseFloat(result);
    else result = result.replace(/'/g, '');
    return result;
}

function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}


const polishedFunctions = ["adjustHue", "complement", "darken", "desaturate", "getLuminance", "grayscale", "hsl", "hsla", "invert", "lighten", "mix", "opacify", "parseToHsl", "parseToRgb",
    "readableColor", "rgb", "rgba", "saturate", "setHue", "setLightness", "setSaturation", "shade", "tint", "transparentize", "animation", "backgroundImages", "backgrounds", "transitions"];

const polishedMixins = ["clearFix", "ellipsis", "fontFace", "hiDPI", "hideText", "hideVisually", "normalize", "placeholder", "radialGradient", "retinaImage", "selection", "timingFunctions",
    "wordWrap", "borderColor", "borderRadius", "borderStyle", "borderWidth", "margin", "padding", "position", "size"];

const polishedHelpers = ["buttons", "textInputs"];