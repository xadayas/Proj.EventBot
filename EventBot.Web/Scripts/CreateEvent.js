function UploadImage() {
    var formData = new FormData();
    var fileUpload = document.getElementById('fileUpload');
    var fileUploadButton = document.getElementById('fileUploadButton');

    formData.append('file', fileUpload.files[0]);
    console.log(fileUpload.files[0]);
    fileUploadButton.value = "Uploading....";
    
    var imgId = document.getElementById('EventImage');
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Images/UploadGetId', true);
    xhr.onload = function() {
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
    console.log("ODG!");
    var title = $('#titleInput').val();
    var desc = $('#descriptionInput').val();
    var place = $('#meetingPlaceInput').val();
    var start = $('#startDateInput').val();
    var end = $('#endDateInput').val();

    var link = $('#EventImageLink').prop('href');

    console.log(start);
    link = link.replace("titleText", title);
    link = link.replace("descriptionText", desc);
    link = link.replace("meetingPlaceText", place);
    link = link.replace("startDateText", start);
    link = link.replace("endDateText", end);

    $('#EventImageLink').prop('href',link);

});

$('.datetimefield').AnyPicker({
    mode: "datetime",
    dateTimeFormat: "yyyy-MM-dd HH:mm",
    showComponentLabel: true,
    parent: "div.container-fluid",
    layout: "fixed",
    vAlign:"top"
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
    console.log(autocomplete);
    console.log(input);
   });
  

