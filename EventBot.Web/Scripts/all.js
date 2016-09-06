//$(document).ready(function () {
//    $.getJSON("/api/notifications/getNewNotifications", function (notifications) {
//        if (notifications.length == 0)
//            return;


//        $(".js-notification-count")
//            .text(notifications.length)
//            .removeClass("hide")
//            .addClass("animated bounceInDown");

//        $(".notifications").popover({
//            html: true,
//            title: "Notifikationer",
//            content: function () {
//                var compiled = _.template($("#notifications-template").html());
//                return compiled({ notifications: notifications });
//            },
//            placement: "bottom",
//            template: '<div class="popover popover-notifications" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
//        }).on("shown.bs.popover", function () {
//            //$.post("/api/notifications/markAllAsRead")
//            //    .done(function () {
//                    $(".js-notification-count")
//                        .text("")
//                        .addClass("hide");
//                //});
//        });
//    });
//});

var MyNotifications;

$(document).ready(function () {
            $.getJSON("/api/Notifications/GetNewNotifications/", function (notifications) {
                if (notifications.length == 0)
                    return;
                MyNotifications = notifications;
                console.log(notifications);
                $("#notificationsbadge")
                    .text(notifications.length)
                    .removeClass("hide")
                    .addClass("animated bounceInDown");

                $(".notifications").popover({
                html: true,
                title: "Notifikationer",
                content: function () {
                    console.log("snopp");
                        var compiled = _.template($("#notifications-template").html());
                        return compiled({ notifications: notifications });
            },
                placement: "bottom",
                template: '<div class="popover popover-notifications" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div><div id="notcontent"></div></div>'

            }).on("shown.bs.popover", function() {
            //    $.post("/api/Notifications/MarkAllAsRead")
            //            .done(function () {
            //                $(".js-notifications-count")
            //                    .text("")
            //                    .addClass("hide");
                //});
                    UpdateNotifications();
                });
            });
});

function UpdateNotifications() {
    $('#notcontent').text('');
    $.each(MyNotifications, function (i, item) {
        if (item.eventType==1) {
            $('#notcontent').append('<li class="notify-li" onClick="OnNotificationClicked(' +item.id +','+ item.eventId+');">Planerade aktiviteten <span class="highlight">'+ item.eventName +'</span> som var planerad '+ moment(item.dateTime).format("D MMM HH:mm")+ ' är tyvärr inställd.</li>');
        }
        else if (item.eventType == 2) {
            var changes = [],
            originalValues = [],
            newValues = [];

            if (item.originalStartDate != item.startDate) {
                changes.push('datum/tid');
                originalValues.push(moment(item.originalStartDate).format("D MMM HH:mm"));
                newValues.push(moment(item.startDate).format("D MMM HH:mm"));

                $('#notcontent').append('<li class="notify-li" onClick="OnNotificationClicked(' + item.id + ',' + item.eventId + ',' + item.eventType + ');"><span class="highlight">' + item.eventName + '</span> har ändrat ' + changes.join(" and ") + ' från ' + originalValues.join("/") + ' till ' + newValues.join("/") + '</li>');
            } else {
                $('#notcontent').append('<li class="notify-li" onClick="OnNotificationClicked(' + item.id + ',' + item.eventId + ',' + item.eventType + ');"><span class="highlight">' + item.eventName + '</span> har uppdaterat aktiviteten som är planerad ' + moment(item.startDate).format("D MMM HH:mm") + '.</li>');
            }
        }
        else if (item.eventType == 4) {//EventJoined
            $('#notcontent').append('<li class="notify-li" onClick="OnNotificationClicked(' + item.id + ',' + item.eventId + ',' + item.eventType + ');"><span class="emo-icon">✌</span> Du har anmält dig till <span class="highlight">' + item.eventName + '! klicka här för att komma till alla dina kommande aktiviteter</span> </li>');
        }
        else if (item.eventType == 5) {//EventLeaved
            $('#notcontent').append('<li class="notify-li" onClick="OnNotificationClicked(' + item.id + ',' + item.eventId + ',' + item.eventType + ');"><span class="emo-icon">👋</span> Du har avanmält dig från <span class="highlight">' + item.eventName + '!</span> </li>');
        }
        else if (item.eventType == 6) {//EventUserHasJoined
            $('#notcontent').append('<li class="notify-li" onClick="OnNotificationClicked(' + item.id + ',' + item.eventId + ',' + item.eventType + ');"><span class="emo-icon">✌</span> Någon har anmält sig till ditt event <span class="highlight">' + item.eventName + '!</span> </li>');
        }
        else if (item.eventType == 7) {//EventUserHasLeaved
            $('#notcontent').append('<li class="notify-li" onClick="OnNotificationClicked(' + item.id + ',' + item.eventId + ',' + item.eventType + ');"><span class="emo-icon">🖕</span> Någon har avanmält sig från ditt event <span class="highlight">' + item.eventName + '!</span> </li>');
        }
        $('#notcontent').append('<hr>');
    });
    $('#notcontent').append('<li style="text-align: center;" onClick="MarkAllAsRead();">Rensa</li>');
}

function OnNotificationClicked(notId, eventId, eventType) {
    console.log(notId + " " + eventId);
    $.post("/api/Notifications/MarkSingleAsRead/"+notId)
                .done(function () {
                    if (eventType == 4) {
                        window.location = "/Event/GetJoinedEvents";
                    } else {
                        window.location = "/Event/Details/" + eventId;
                    }
                   
                });
}
function MarkAllAsRead() {
        $.post("/api/Notifications/MarkAllAsRead")
                .done(function () {
                    $("#notificationsbadge")
                        .text("")
                        .addClass("hide");
                $('.notifications').popover('disable');
                MyNotifications = "";
                    $('#notcontent').text('');
                $('#notificationsbadge').click();
            });
}
function NotificationTypeToString(ntype) {
    
    if (ntype === 1)
        return "Cancelled";
    else if (ntype === 2)
        return "Updated";
    else if (ntype === 3)
        return "Created";
    else {
        return "wat";
    }
}
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

//$(document).on("scroll", function () {
//    if ($(document).scrollTop() > 100) {
//        $("header").removeClass("large").addClass("small");
//    } else {
//        $("header").removeClass("small").addClass("large");
//    }
//});