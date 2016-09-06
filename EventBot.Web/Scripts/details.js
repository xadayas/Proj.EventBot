
var IsAttending = false;
var AvailableSlots = 0;
var eventId = 0;

$('.js-cancel-event').click(function (e) {
    var link = $(e.target);
    console.log(link.attr("data-event-id"));
    bootbox.dialog({
        message: "Är du säker på att du vill ställa in aktiviteten?",
        title: "Ställ in",
        buttons: {
            no: {
                label: "Nej",
                className: "btn-default",
                callback: function () {
                    bootbox.hideAll();
                }
            },
            yes: {
                label: "Ja",
                className: "btn-danger",
                callback: function () {
                    $.ajax({
                        url: "/api/events/cancel/" + link.attr("data-event-id"),
                        method: "DELETE"
                    })
                    .done(function () {
                        window.location.reload();
                        //window.location.href = "/event/userevents";
                    })
                    .fail(function () {
                        alert("Obs, något blev fel!");
                    });
                }
            }
        }
    });
})

function OnReady() {
    eventId = $("#hdnVal").val();
    buildButton();
    $('#eventTags').tagsinput();
    setInterval(checkIsAttending, 5000);
}

function buildButton() {
    checkIsAttending();

    var jl = IsAttending ? "Lämna" : "Gå med";
    if (AvailableSlots == 0 && !IsAttending)
        jl = "Inga platser kvar!";
    $("#attend").empty();
    var joinorleave = '<input type="submit" class="btn btn-default" id="bt" value=' + jl + "></input>";
    $(joinorleave).appendTo('#attend');
    $('#bt').click(function (event) {
        event.preventDefault();
        JoinOrLeave();
        return false;
    });
    //if(isdisabled)
    //    //$('#bt').prop('disabled', true);
    //else 
    //    $('#bt').prop('disabled', false);

    $("#alertbox").empty();
}
function checkIsAttending() {
    console.log("checking...");
    if (eventId == null)return;
    $.getJSON('/participants/isattending/' + eventId, function (attendStatus) {
        IsAttending = attendStatus.Attending;
        AvailableSlots = attendStatus.AvailableSlots;
        var jl = IsAttending ? "Lämna" : "Gå med";
        if (AvailableSlots == 0 && !IsAttending)
            jl = "Inga platser kvar!";
        $("#bt").val(jl);
    });
}

function ToFunctionName() {
    return IsAttending ? "UnAttend" : "Attend";
}
function JoinOrLeave() {
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: '/Participants/' + ToFunctionName()+'/'+eventId,
        statusCode: {
            200: function(response) {
                buildButton();
                SetAlert(false);
            },
            409: function (response) {
                buildButton(true);
                SetAlert(true);
            }
        }
    });
}
function SetAlert(errorOnAttend) {
    $("#alertbox").empty();
    var joined = '<div class="alert alert-success alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Välkommen!</strong> Kom ihåg att inte glömma något du behöver ta med dig!</div>';
    var leaved = '<div class="alert alert-warning alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Glöm inte bort!</strong> Att det är bara att anmäla sig igen om du ändrar dig! :)</div>';
    var unavailable = '<div class="alert alert-danger alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Det finns inga lediga platser!</strong> Du får försöka lite senare, så kanske någon har lämnat en plats! </div>';
    if (errorOnAttend)
        $("#alertbox").append(unavailable);
    else if (!IsAttending)
        $("#alertbox").append(joined);
    else if(IsAttending) {
        $("#alertbox").append(leaved);
    }
}