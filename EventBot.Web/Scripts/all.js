﻿$(document).ready(function () {
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
  