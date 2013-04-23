
; ------------------------
; winrt - demo snippits
; ------------------------

::d11::
clipboard =
(
	<button id="myButton">Make Call</button>
	<textarea aria-multiline="true" id="results" aria-readonly="True">results here..</textarea>
)
send ^v
return

::d12::
clipboard =
(
	public static string HelloWorld(){
		return "hello world";
	}
)
send ^v
return

::d13::
clipboard =
(
	function buttonClick () {

    };
)
send ^v
return

::d14::
clipboard =
(
	app.onready = function () {
        document.getElementById("myButton").addEventListener("click", buttonClick, false);
    }
)
send ^v
return

::d15::
clipboard =
(
	var resultElement = document.getElementById("results");

	var existingText = resultElement.innerHTML;
	var x = WinRTComponent.MyWinRTAwesome.helloWorld();

	resultElement.innerHTML = existingText + "\n" + x;
)
send ^v
return

; ------------------------
; web - demo snippits
; ------------------------

::d21::
clipboard =
(
	<script src="~/Scripts/jquery-1.7.1.js"></script>
	<script src="~/Scripts/knockout-2.1.0.js"></script>
)
send ^v
return

::d22::
clipboard =
(
    <script type="text/html" id="speaker-template">
		<div class="item">

            <h2 class="title"><span data-bind="text: FirstName"></span> <span data-bind="text: LastName"></span></h2>
		    <p class="desc" data-bind="text: Bio"></p>
            <div class="pic"></div>
		    
        </div>
	</script>
)
send ^v
return

::d23::
clipboard =
(
	<header>Speakers</header>
	
	<div id="content" data-bind="template: { name: 'speaker-template', foreach: d }"></div>
)
send ^v
return

::d24::
clipboard =
(	
	<script type="text/javascript">
        $(document).ready(function () {
            $.ajax({
                url: 'http://www.thatconference.com/odata/api.svc/People',
                dataType: "json",

                success: function (data) {
                    ko.applyBindings(data);
                },
            });
        });
	</script>
)
send ^v
return

::d25::
clipboard =
(	
        WinJS.xhr({
            url: "http://thatConference.com/odata/api.svc/People",
            headers: { accept: "application/json" }

        }).then(
            function (args) {
                var obj = JSON.parse(args.responseText);
                ko.applyBindings(obj);
            },
            function (args) {
                //error
            });	
			
)
send ^v
return

; ------------------------
; app bar - demo snippits
; ------------------------

::ab1::
clipboard =
(	
    <div id="appbar" data-win-control="WinJS.UI.AppBar">            
            
        <button data-win-control="WinJS.UI.AppBarCommand" 
                data-win-options="{id:'cmd', label:'picture', icon:'placeholder'}" 
                type="button"
                id="picture">
        </button>

    </div>
)
send ^v
return

::ab2::
clipboard =
(	
    document.querySelector("#picture").addEventListener("click", imageCapture);
)
send ^v
return

::ab3::
clipboard =
(	
	
	function imageCapture() {
        var _capture = Windows.Media.Capture;

        var captureUI = new _capture.CameraCaptureUI();

        captureUI.photoSettings.format = _capture.CameraCaptureUIPhotoFormat.png;
        captureUI.photoSettings.croppedAspectRatio = { height: 4, width: 3 };

        captureUI.captureFileAsync(_capture.CameraCaptureUIMode.photo)
            .then(function (capturedItem) {
                if (capturedItem) {

                    var photoBlobUrl = URL.createObjectURL(
                        capturedItem,
                        { oneTimeOnly: true });

                    var imageElement = document.createElement("img");
                    imageElement.setAttribute("src", photoBlobUrl);

                    document.querySelector(".pic").appendChild(imageElement);
                }
            });
    }
	
)
send ^v
return

; ------------------------
; settings snippits
; ------------------------

::ss1::
clipboard =
(	
	app.onsettings = function (e) {

        e.detail.applicationcommands = {
            "about": {
                href: "/pages/settings/aboutflyout.html",
                title: "not about"
            },
            "help": {
                href: "/pages/settings/helpFlyout.html",
                title: "Help"
            }
        }

        WinJS.UI.SettingsFlyout.populateSettings(e);
    };
	
)
send ^v
return

::css1::
clipboard = 
(

body {

	height: 100`%;
	width: 100`%;

	overflow-y: scroll;

	display: -ms-grid;
	-ms-grid-columns: 120px 1fr 120px;
	-ms-grid-rows: 120px 1fr 120px;
} 

)
send ^v
return

::css2::
clipboard =
(	

header {
	-ms-grid-column: 2;
	-ms-grid-row: 1;
	
	font-size: 500`%;
} 

)
send ^v
return

::css3::
clipboard =
(	

#content {
	-ms-grid-column: 2;
	-ms-grid-row: 2;

	display: -ms-box;
	-ms-flex-align: middle;
	-ms-flex-pack: justify;
	-ms-flex-direction: normal;
	-ms-flex-wrap: normal;
}

)
send ^v
return

::css4::
clipboard =
(	

.item {
	display: -ms-grid;
	-ms-grid-columns: 1fr 120px;
	-ms-grid-rows: 25px 1fr;
	
	margin-bottom: 10px;
}

)
send ^v
return

::css5::
clipboard =
(	

.title {
	-ms-grid-row: 1;
	-ms-grid-column: 1;
}

)
send ^v
return

::css6::
clipboard =
(	

.desc {
	-ms-grid-row: 2;
	-ms-grid-column: 1;
	padding-left: 25px;
}

)
send ^v
return

::css7::
clipboard =
(	

.pic {
	-ms-grid-row-span: 2;
	-ms-grid-column: 2;
	
	width: 100px;
}

)
send ^v
return

::css8::
clipboard =
(	

@media screen and (-ms-view-state: fullscreen-landscape) {
	
}

@media screen and (-ms-view-state: filled) {
	body {
        background-color: dimgrey;
    }
}

@media screen and (-ms-view-state: snapped) {
    body {
        background-color: blue;
    
    	display: -ms-grid;
	    -ms-grid-columns: 5px 1fr 5px;
	    -ms-grid-rows: 120px 1fr 120px;
    }
}

@media screen and (-ms-view-state: fullscreen-portrait) {
}

)
send ^v
return