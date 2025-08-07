export function getQueryString() {
    return window.location.search;
}

export function getQueryStringParameter(name) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}

export function getQueryStringParameters() {
    const urlParams = new URLSearchParams(window.location.search);
    const params = {};
    for (const [key, value] of urlParams.entries()) {
        params[key] = value;
    }
    return params;
}

export function getHostAddress() {
    return window.location.origin;
}
