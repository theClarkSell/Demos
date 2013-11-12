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
        document.querySelector("#btnEnableGestures").addEventListener("click", enableGestures, false);
        document.querySelector("#btnDisableGestures").addEventListener("click", disableGestures, false);
    };
    
    function enableGestures(args) {
        var gestureHelper = new WinMD.GestureHelper();
        var helperResults = gestureHelper.enableGestures();

        callResults.textContent = helperResults;
    };

    function disableGestures(args) {
        var gestureHelper = new WinMD.GestureHelper();
        var helperResults = gestureHelper.disableGestures();

        callResults.textContent = helperResults;
    };

    app.start();

})();
