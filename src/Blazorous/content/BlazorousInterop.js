window.blazorous = {
    css: function (cssString, debug) {
        if (debug) console.log("Css String: %o", cssString);
        const cssPolished = parsePolishedFunctions(cssString, debug);
        const cssJson = JSON.parse(cssPolished);
        if(debug) console.log("Css Json: %o", cssJson);
        const rule = Glamor.css(cssJson);
        return rule.toString();
    },
    keyframes: function (keyframes, debug) {
        if(debug) console.log("Keyframes: %o", keyframes);
        const cssKeyframes = JSON.parse(keyframes);
        if (debug) console.log("Css Keyframes: %o", cssKeyframes);
        const animation = Glamor.css.keyframes(cssKeyframes);
        return animation;
    },
    fontface: function (fontface, debug) {
        if (debug) console.log("fontface: %o", fontface);
        const cssFontface = JSON.parse(fontface);
        if (debug) console.log("cssFontface: %o", cssFontface);
        const family = Glamor.css.fontFace(cssFontface);
        return family;
    },
    polishedMixin: function (strMixin, debug) {
        if (debug) console.log("Mixin string: %o", strMixin);
        const result = parsePolishedMixins(strMixin, debug)
        const fixedObjName = fixObjectNames(result);
        const resultString = JSON.stringify(fixedObjName);
        if (debug) console.log("Mixin result string: %o", resultString);
        return resultString;
    }
};

function parsePolishedMixins(cssString, debug) {
    let result = cssString;
    let found = false;
    let i = 0;
    while (true) {
        const f = polishedMixins[i];
        if (result.indexOf(f + "(") !== -1) {
            const start = result.indexOf(f + "(");
            const end = getClosingParens(result, start);
            const methodWithParams = result.substring(start, end);
            const method = methodWithParams.substring(0, methodWithParams.indexOf("("));
            const params = methodWithParams.substring(methodWithParams.indexOf("(") + 1, methodWithParams.length - 1);
            const paramsParsed = parsePolishedFunctions(params, debug);
            console.log("paramsParsed:", paramsParsed);
            if (paramsParsed.startsWith('{') && paramsParsed.endsWith('}')) {
                const obj = JSON.parse(paramsParsed);
                console.log("obj: %o", obj);
                result = polished[method](obj);
            } else {
                const list = paramsParsed.split(",");
                const args = []
                list.forEach(a => args.push(normalizeArg(a)));
                result = polished[method].apply(this, args);
            }
            if (debug) console.log("Polished mixin result: ", result);
            found = true;
        }
        i++;
        if (found || i >= polishedMixins.length) break;
    }
    return result;
}

function fixObjectNames(obj) {
    const result = {};
    Object.keys(obj).forEach(function (key) {
        result[camelToHyphen(key)] = obj[key];
    });
    return result
}

function camelToHyphen(str) {
    return str.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
}


function parsePolishedFunctions(cssString, debug) {
    let result = cssString;
    let found = false;
    let loopStart = 0;
    polishedFunctions.forEach(f => {
        loopStart = 0;
        while (true) {
            found = false;
            if (result.indexOf(f + "(", loopStart) !== -1) {
                const start = result.indexOf(f + "(", loopStart);
                loopStart = start;
                const end = getClosingParens(result, start);
                const methodWithParams = result.substring(start, end);
                if (debug) console.log("Polished string: ", methodWithParams);
                const method = methodWithParams.substring(0, methodWithParams.indexOf("("));
                const params = methodWithParams.substring(methodWithParams.indexOf("(") + 1, methodWithParams.length - 1);
                const paramsParsed = parsePolishedFunctions(params);
                const list = paramsParsed.split(",");
                const args = []
                list.forEach(a => args.push(normalizeArg(a)));
                const polishedResult = polished[method].apply(this, args);
                loopStart += polishedResult.length;
                if(debug) console.log("Polished result: ", polishedResult);
                result = result.replace(methodWithParams, polishedResult);
                found = true;
            }
            if (!found || loopStart > result.length) break;
        }
    });
    return result;
}

function getClosingParens(strIn, start) {
    const str = strIn.substring(start);
    const chars = str.split('');
    let i = 0;
    let count = 0;
    while (true) {
        const char = str.charAt(i);
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
    let result = arg.trim();
    if (isNumeric(result)) result = parseFloat(result);
    else {
        result = result.replace(/'/g, '');
        if (result === "null") result = null;
    }
    return result;
}

function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

const polishedFunctions = ["adjustHue", "complement", "darken", "desaturate", "getLuminance", "grayscale", "hsl", "hsla", "invert", "lighten", "mix", "opacify", "parseToHsl", "parseToRgb",
    "readableColor", "rgb", "rgba", "saturate", "setHue", "setLightness", "setSaturation", "shade", "tint", "transparentize", "animation", "backgroundImages", "backgrounds", "transitions"];

const polishedMixins = ["clearFix", "ellipsis", "fontFace", "hiDPI", "hideText", "hideVisually", "normalize", "radialGradient", "retinaImage", "selection", "timingFunctions",
    "wordWrap", "borderColor", "borderRadius", "borderStyle", "borderWidth", "margin", "padding", "position", "size"];

const polishedHelpers = ["buttons", "placeholder", "textInputs"];