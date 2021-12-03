$("#selectImage").change(function () {
    var fileName = this.files[0].name;
    var fileSize = this.files[0].size;
    var isValidFile = 0;

    if (fileSize > 5000000) {
        alert('The file must not exceed 5MB');
        this.value = '';
        this.files[0].name = '';
        isValidFile = 1;
    } else {
        var ext = fileName.split('.').pop();
        ext = ext.toLowerCase();

        switch (ext) {
            case 'jpg':
            case 'jpeg':
            case 'png': break;
            default:
                alert('The file does not have the correct extension.');
                this.value = '';
                this.files[0].name = '';
                isValidFile = 1;
        }
    }
    if (isValidFile == 0) {
        readURL(this);
    }

});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $("#image").attr("src", e.target.result);
            $("#image").attr("display", "block");
        }

        reader.readAsDataURL(input.files[0]);
    }
}