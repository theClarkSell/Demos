$(function () {
    
    //DEFAULT IE9IFY ACTIONS.
    $('head').ie9ify({
        navColor: 'Yellow',
        tasks: [
            {
                'name': 'Galleries',
                'action': '/Gallery',
                'icon': '/Content/Images/icon-gallery.ico'
            },
            {
                'name': 'Tags',
                'action': '/Tag',
                'icon': '/Content/Images/icon-tags.ico'
            },
            {
                'name': 'Account',
                'action': '/User',
                'icon': '/Content/Images/icon-account.ico'
            }
        ]
    });

    //ADD A SITE LOGO
    $('#site-logo').ie9ify('enablePinning');

    //ADD A TEASER TO THE WEBPAGE
    $('#header').ie9ify('pinTeaser', {
        addStartLink: false
    });

    //CHECK TO SEE IF THE SITE IS ALREADY PINNED
    if ($.ie9ify.isPinned()) {

        //IF PINNED, MAKE A AJAX CALL TO GET A JUMPLIST OF ITEMS
        $.getJSON('Gallery/List', function (data) {
            var itemList = [];

            $.each(data, function (key, val) {
                var item = {
                    'name': data[key].name,
                    'url': 'Gallery/View/' + data[key].id,
                    'icon': '/Content/Images/icon-gallery.ico'
                };

                itemList.push(item);
            });

            //ADD THAT LIST TO THE JUMPLISTS
            $.ie9ify.addJumpList({
                title: 'Photo Galleries',
                items: itemList
            });
        });

        //IF PINNED CREATE SOME THUMBAR BUTTONS
        $.ie9ify.createThumbbarButtons({
            buttons: [{
                icon: '/Content/Images/download.ico',
                name: 'Download Images',
                click: function () {
                    document.location = 'Gallery';
                }
            },
                {
                    icon: '/Content/Images/icon-tags.ico',
                    name: 'Tag Images',
                    click: function () {
                        document.location = 'Tag';
                    }
                }]
        });

        //IF PINNED, SET AN INTERVAL THAT WE WILL CALL THE WEBSERVICE LOOKING FOR COMMENTS.  
        //IF THERE IS ONE THEN OVERLAY AN ICON
        setInterval(function () {
            $.getJSON('Photo/CommentsForUser/' + new Date().getTime(), function (data) {
                $.ie9ify.clearOverlay();

                var itemList = [];
                var commentCount = parseInt($('#commentCount').val());

                if (data.length > commentCount) {
                    $.ie9ify.flashTaskbar();
                    $('#commentCount').val(data.length);

                    $.ie9ify.addOverlay({
                        title: 'New comment: ' + data[0].text + ' (' + data[0].date + ')',
                        icon: '/Content/Images/comment.ico'
                    });
                }
            });
        }, 5000);
    }
});