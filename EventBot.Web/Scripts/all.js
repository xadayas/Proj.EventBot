$(document).ready(function () {
            $.getJSON("/api/notifications", function (notifications) {
                if (notifications.length == 0)
                    return;

                $(".js-notifications-count")
                    .text(notifications.length)
                    .removeClass("hide")
                    .addClass("animated bounceInDown");

                $(".notifications").popover({
                html: true,
                title: "Notifikationer",
                content: function () {
                        var compiled = _.template($("#notifications-template").html());
                        return compiled({ notifications: notifications });
            },
                placement: "bottom",
                template: '<div class="popover popover-notifications" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'

            }).on("shown.bs.popover", function() {
                    $.post("/api/notifications/markAllAsRead")
                        .done(function () {
                            $(".js-notifications-count")
                                .text("")
                                .addClass("hide");
            });
            });
            });
});
$.getScript('https://www.google.com/jsapi', function () {
    google.load('maps', '3', {
        callback: function () {
            geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': your_address }, function (results, status) {
                if (status === google.maps.GeocoderStatus.OK) {
                    var lat = results[0].geometry.location.lat();
                    var lng = results[0].geometry.location.lng();
                    var options = { zoom: 10, center: { lat: lat, lng: lng } };
                    var map = new google.maps.Map(document.getElementById('map-container'), options);
                    new google.maps.Marker({ position: { lat: lat, lng: lng }, map: map });
                } else {
                    alert("Geocode was not successful for the following reason: " + status);
                }
            });

        }
    });
});