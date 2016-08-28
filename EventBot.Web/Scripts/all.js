$(document).ready(function () {
    $.getJSON("/api/notifications/getNewNotifications", function (notifications) {
        if (notifications.length == 0)
            return;

        console.log(notifications);
        $(".js-notification-count")
            .text(notifications.length)
            .removeClass("hide")
            .addClass("animated bounceInDown");
        console.log("end of method");

        $(".notifications").popover({
            html: true,
            title: "Notifikationer",
            content: function () {
                var compiled = _.template($("#notifications-template").html());
                return compiled({ notifications: notifications });
            },
            placement: "bottom",
            template: '<div class="popover popover-notifications" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
        }).on("shown.bs.popover", function () {
            $.post("api/notifications/markAllAsRead")
                .done(function () {
                    $(".js-notification-count")
                        .text("")
                        .addClass("hide");
                });
        });
    });
});

//var MyNotifications;

//$(document).ready(function () {
//            $.getJSON("/api/Notifications/GetNewNotifications/", function (notifications) {
//                if (notifications.length == 0)
//                    return;
//                MyNotifications = notifications;
//                console.log(notifications);
//                $("#notificationsbadge")
//                    .text(notifications.length)
//                    .removeClass("hide")
//                    .addClass("animated bounceInDown");

//                $(".notifications").popover({
//                html: true,
//                title: "Notifikationer",
//                content: function () {
//                        var compiled = _.template($("#notifications-template").html());
//                        return compiled({ notifications: notifications });
//            },
//                placement: "bottom",
//                template: '<div class="popover popover-notifications" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div><div id="notcontent"></div></div>'

//            }).on("shown.bs.popover", function() {
//            //    $.post("/api/Notifications/MarkAllAsRead")
//            //            .done(function () {
//            //                $(".js-notifications-count")
//            //                    .text("")
//            //                    .addClass("hide");
//                //});
//                    UpdateNotifications();
//                });
//            });
//});

//function UpdateNotifications() {
//    $.each(MyNotifications, function (i, item) {
//        $('#notcontent').append('<div class="row"><button type="button" class="btn btn-default btn-block" onClick="OnNotificationClicked(' + item.id+","+item.eventId + ')">' + item.eventName +" " +NotificationTypeToString(item.eventType)+ "</button> </div>");
//    });
//}

//function OnNotificationClicked(notId, eventId) {
//    console.log(notId + " " + eventId);
//    $.post("/api/Notifications/MarkSingleAsRead/"+notId)
//                .done(function () {
//                    window.location = "/event/details/" + eventId;
//                });
//}
//function NotificationTypeToString(ntype) {
    
//    if (ntype === 1)
//        return "Cancelled";
//    else if (ntype === 2)
//        return "Updated";
//    else if (ntype === 3)
//        return "Created";
//    else {
//        return "wat";
//    }
//}
//$.getScript('https://www.google.com/jsapi', function () {
//    google.load('maps', '3', {
//        callback: function () {
//            geocoder = new google.maps.Geocoder();
//            geocoder.geocode({ 'address': your_address }, function (results, status) {
//                if (status === google.maps.GeocoderStatus.OK) {
//                    var lat = results[0].geometry.location.lat();
//                    var lng = results[0].geometry.location.lng();
//                    var options = { zoom: 10, center: { lat: lat, lng: lng } };
//                    var map = new google.maps.Map(document.getElementById('map-container'), options);
//                    new google.maps.Marker({ position: { lat: lat, lng: lng }, map: map });
//                } else {
//                    alert("Geocode was not successful for the following reason: " + status);
//                }
//            });

//        }
//    });
//});

$(document).on("scroll", function () {
    if ($(document).scrollTop() > 100) {
        $("header").removeClass("large").addClass("small");
    } else {
        $("header").removeClass("small").addClass("large");
    }
});