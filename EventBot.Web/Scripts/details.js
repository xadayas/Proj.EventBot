
var IsAttending = false;
var eventId = 0;

function OnReady() {
    eventId = $("#hdnVal").val();
    buildButton();
    $('#eventTags').tagsinput();
}

function buildButton() {
    checkIsAttending(eventId);

    var jl = IsAttending ? "Lämna" : "Gå med";
    $("#attend").empty();
    var joinorleave = '<input type="submit" class="btn btn-default" id="bt" value=' + jl + "></input>";
    $(joinorleave).appendTo('#attend');
    $('#bt').click(function (event) {
        event.preventDefault();
        JoinOrLeave();
        return false;
    });
    $("#alertbox").empty();
}
function checkIsAttending(eventId) {
    if (eventId == null)return;
    $.getJSON('/participants/isattending/' + eventId, function (attendStatus) {
        IsAttending = attendStatus.Attending;
        var jl = IsAttending ? "Lämna" : "Gå med";
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
            buildButton();
            checkIsAttending(eventId);
            SetAlert();
            
        },
        fail: function (data) {
            console.log("NO");
            checkIsAttending(eventId);
        }
    });
}
function SetAlert() {
    $("#alertbox").empty();
    var joined = '<div class="alert alert-success alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Välkommen!</strong> Kom ihåg att inte glömma något du behöver ta med dig!</div>';
    var leaved = '<div class="alert alert-warning alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Glöm inte bort!</strong> Att det är bara att anmäla sig igen om du ändrar dig! :)</div>';
    if (!IsAttending)
        $("#alertbox").append(joined);
    else {
        $("#alertbox").append(leaved);
    }

}