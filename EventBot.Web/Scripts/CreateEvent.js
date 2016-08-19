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