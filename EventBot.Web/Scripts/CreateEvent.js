function UploadImage() {
    var formData = new FormData();
    var fileUpload = document.getElementById('fileUpload');
    var fileUploadButton = document.getElementById('fileUploadButton');

    formData.append('file', fileUpload.files[0]);
    fileUploadButton.value = "Uploading....";

    var imgId = document.getElementById('EventImage');
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Images/UploadGetId', true);
    xhr.onload = function () {
        if (xhr.status === 200) {
            fileUploadButton.value = "Upload";
            document.getElementById('EventImage').src = '/Images/View/' + xhr.responseText;
            document.getElementById('ImageId').value = xhr.responseText;
        } else {
            alert('Error Uploading');
        }
    };
    xhr.send(formData);
}

$('#EventImageLink').click(() => {

    // Redirect form to /images/upload
    $('#eventForm').action = "/Images/Upload";
    $('#eventForm').prop('method', 'GET');
    $('#eventForm').prop('action', '/images/upload');

    // disable validation to be able to send incomplete form
    $('.form-control').attr('data-val',false);
    $('#eventForm').removeData('unobtrusiveValidation');
    $('#eventForm').removeData('validator');
    $.validator.unobtrusive.parse('#eventForm');

    // submit form
    $('#eventForm').submit();
    

});

$('.datetimefield').AnyPicker({
    mode: "datetime",
    dateTimeFormat: "yyyy-MM-dd HH:mm",
    showComponentLabel: true,
    parent: "div.form-horizontal",
    layout: "fixed",
    vAlign: "top"
});


window.addEventListener("load", () => {
    var defaultBounds = new google.maps.LatLngBounds(
      new google.maps.LatLng(53, 0),
      new google.maps.LatLng(70, 27));

    var input = document.getElementById('meetingPlaceInput');
    var options = {
        bounds: defaultBounds
    };

    autocomplete = new google.maps.places.Autocomplete(input, options);
    $('#eventTags').tagsinput({
        typeahead: {
            afterSelect: function (val) { this.$element.val(""); },
            source: (query) => {return $.get('/api/tags/GetAllTags/');}
        }
    });
});


