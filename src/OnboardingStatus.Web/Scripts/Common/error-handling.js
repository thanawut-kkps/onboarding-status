window.onerror = function (errorMsg, url, lineNumber, column, errorObj) {
    if (errorMsg.indexOf('Script error.') > -1) {
        return;
    }
    var errorMessage = 'Error: ' + errorMsg + '<br>Script: ' + url + '<br>Line: ' + lineNumber + '<br>Column: ' + column + '<br>StackTrace: ' + errorObj;
    if (window) {
        if (window.location) {
            if (window.location.href) {
                errorMessage = 'URL:' + window.location.href + ' ' + errorMessage;
            }
        }
    }
    dialog.sendErrorMessageToServer(errorMessage);
}


    




