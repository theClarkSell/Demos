// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
    };

    app.onready = function (args) {
        document.querySelector("#btnDisableViaDirect").addEventListener("click", directDisable, false);
        document.querySelector("#btnDisableViaEnum").addEventListener("click", enumDisable, false);
    };
    
    function directDisable(args) {
        var gestureHelper = new WinMD.GestureHelper();
        var helperResults = gestureHelper.disableViaDirect();

        callResults.textContent = "PID: " + helperResults;
    };

    function enumDisable(args) {
        var gestureHelper = new WinMD.GestureHelper();
        var helperResults = gestureHelper.disableViaEnum();

        callResults.textContent = "PID: " + helperResults;
    };

    app.start();

})();
