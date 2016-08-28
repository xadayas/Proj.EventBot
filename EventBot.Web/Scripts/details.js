
var IsAttending = false;
var eventId = 0;
$(document).ready(function () {
    eventId = $("#hdnVal").val();
    checkIsAttending(eventId);

    var jl = IsAttending ? "Leave" : "Join";
    var joinorleave = '<input type="submit" id="bt" value=' + jl + "></input>";
    $(joinorleave).appendTo('#attend');
    $('#bt').click(function (event) {
        event.preventDefault();
        JoinOrLeave();
        return false;
    });
});
function checkIsAttending(eventId) {
    if (eventId == null)return;
    $.getJSON('/participants/isattending/' + eventId, function (attendStatus) {
        IsAttending = attendStatus.Attending;
        var jl = IsAttending ? "Leave" : "Join";
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
        success: function (data) {
            checkIsAttending(eventId);
        },
        fail: function (data) {
            console.log("NO");
            checkIsAttending(eventId);
        }
    });
}