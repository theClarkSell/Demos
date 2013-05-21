
; <><><><><><><><><><><><><><><>
; WEB/W8 - demo snippits
; <><><><><><><><><><><><><><><>

; ------------------------------------------------
; open folder in webmatrix
; copy the scripts and folders over in powershell
; ------------------------------------------------

; ps: webCopy.ps1

; ------------------------------------------------
; Add the scripts to the page
; ------------------------------------------------
::d21::
clipboard =
(
		<script src="/scripts/jquery-2.0.0.min.js"></script>
	    <script src="/scripts/knockout-2.2.1.js"></script>
)
send ^v
return

; ------------------------------------------------
; Add the knockout template
; ------------------------------------------------

::d22::
clipboard =
(
		<script type="text/html" id="speaker-template">
		    <div class="item">

                <h2 class="title">
                    <span data-bind="text: FirstName"></span> <span data-bind="text: LastName"></span>
                </h2>
		        
                <p class="desc" data-bind="text: Bio"></p>

            </div>
	    </script>
)
send ^v
return

; ------------------------------------------------
; Reference the Knockout Template.
; ------------------------------------------------

::d23::
clipboard =
(
		<header>Speakers</header>
	    
		<div id="content" data-bind="template: { name: 'speaker-template', foreach: d }"></div>

)
send ^v
return

; ------------------------------------------------
; Add the JavaScript function to call out to That Conference
; ------------------------------------------------

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
                    }
                });
            });
	    </script>
)
send ^v
return

; ------------------------------------------------
; Add a styles.css and style
; ------------------------------------------------

::d25::
clipboard =
(	
@font-face {
    font-family: "Shadows";
    src: url("/fonts/ShadowsIntoLight.woff") format('woff');
}

@font-face {
    font-family: "Source";
    src: url("/fonts/SourceCodePro-Regular.woff") format('woff');
}

header, h1, h2 {
    font-family: 'Shadows', cursive; 
}

header {
    font-size: 4em;
    background-color: white;
    color: white;
    text-shadow: black 2px 2px 10px;
}

#content {
    margin: 25px;
}

.desc {
    font-family: 'Source', sans-serif;
    margin:  25px;
}		
)
send ^v
return

; ------------------------------------------------
; Reference the stylesheet
; ------------------------------------------------

::d26::
clipboard =
(	
	<link href="/style.css" rel="stylesheet" />
)
send ^v
return

; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; Redo the entire demo but start from file new.
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>

; ------------------------------------------------
; open base solution
; copy the scripts and folders over in powershell
; ------------------------------------------------

; ps: w8Copy.ps1

; ------------------------------------------------
; Add the onloaded event for the winjs call
; ------------------------------------------------

::d27::
clipboard =
(	
    app.onloaded = function () {
	
	}
)
send ^v
return

; ------------------------------------------------
; Convert the jQuery Call to WinJS
; ------------------------------------------------

::d28::
clipboard =
(	
        WinJS.xhr({
            url: "http://www.thatConference.com/odata/api.svc/People",
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

; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; app bar - demo snippits
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>

; ------------------------------------------------
; Add the app bar. Talk about the data- control
; ------------------------------------------------

::ab1::
clipboard =
(	
    <div id="appbar" data-win-control="WinJS.UI.AppBar">            
            
    </div>
)
send ^v
return

; ------------------------------------------------
; Add the app bar button.
; ------------------------------------------------

::ab2::
clipboard =
(	            
        <button data-win-control="WinJS.UI.AppBarCommand" 
                data-win-options="{id:'cmd', label:'picture', icon:'placeholder'}" 
                type="button"
                id="picture">
        </button>

)
send ^v
return


; ------------------------------------------------
; Add the event listener for the app bar button
; ------------------------------------------------

::ab3::
clipboard =
(	
    document.querySelector("#picture").addEventListener("click", imageCapture);
)
send ^v
return

; ------------------------------------------------
; Integrate with Windows.Media.Capture
; ------------------------------------------------

::ab4::
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

; ------------------------------------------------
; Add the div for the captured picture
; ------------------------------------------------

::ab5::
clipboard =
(	
    <div class="pic"></div>
)
send ^v
return


; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; Settings Settings Settings Settings Settings Settings
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>


; ------------------------------------------------
; Create the settings buttons, files are already there.
; ------------------------------------------------

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


; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; STYLE STYLE STYLE STYLE STYLE STYLE STYLE STYLE STYLE STYLE
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
; <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>

; ------------------------------------------------
; style body
; ------------------------------------------------

::css1::
clipboard = 
(

@font-face {
    font-family: "Shadows";
    src: url("/fonts/ShadowsIntoLight.woff") format('woff');
}

@font-face {
    font-family: "Source";
    src: url("/fonts/SourceCodePro-Regular.woff") format('woff');
}

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

; ------------------------------------------------
; style header
; ------------------------------------------------


::css2::
clipboard =
(	

header, h1, h2 {
    font-family: 'Shadows', cursive;
}

header {
	-ms-grid-column: 2;
	-ms-grid-row: 1;
	
	font-size: 500`%;
	text-shadow: black 2px 2px 10px;
} 

)
send ^v
return

; ------------------------------------------------
; style the content
; ------------------------------------------------

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

; ------------------------------------------------
; style items
; ------------------------------------------------

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

; ------------------------------------------------
; style title
; ------------------------------------------------

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

; ------------------------------------------------
; style the description
; ------------------------------------------------

::css6::
clipboard =
(	

.desc {
	font-family: 'Source', sans-serif;
	-ms-grid-row: 2;
	-ms-grid-column: 1;
	padding-left: 25px;
}

)
send ^v
return

; ------------------------------------------------
; style the pic
; ------------------------------------------------

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

; ------------------------------------------------
; Add the Media 
; ------------------------------------------------

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