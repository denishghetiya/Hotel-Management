$(document).ready(function () {

    $("#loginform").validate({
        rules: {
            Email: { required: true},
            Password: { required: true}
        },
        messages: {
            Email: {
                required: "Email is Required."
            },
            Password: {
                required: "Password is Required."
            }
        },
        errorElement: "div",
        errorClass: "text-danger small mt-1",
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
        },
        submitHandler: function (form) {
            var formData = $(form);
            $('#loginformbtn').prop("disabled", true);
            $.ajax({
                url: "/Login/Login",
                type: "POST",
                data: formData.serialize(),
                success: function (response) {
                    $('#loginformbtn').prop("disabled", false);
                    if(response.success == true){
                        window.location.href = "/Dashboard/Dashboard";
                    }
                    if(response.success == false){
                        Swal.fire({
                          icon: "error",
                          text: response.message
                        });
                    }
                },
                error: function (res) {
                    Swal.fire({
                        icon: "error",
                        text: "Something went wrong: " + res.statusText
                    });
                }
            });
        }
    });

    $("#registerform").validate({
        rules: {
            Username: { required: true},
            Email: { required: true},
            Password: { required: true},
            ConfirmPassword: { required: true},
            Image: { required: true}
        },
        messages: {
            Username: {
                required: "Username is Required."
            },
            Email: {
                required: "Email is Required."
            },
            Password: {
                required: "Password is Required."
            },
            ConfirmPassword: {
                required: "ConfirmPassword is Required."
            },
            Image: {
                required: "Image is Required."
            }
        },
        errorElement: "div",
        errorClass: "text-danger small mt-1",
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
        },
        submitHandler: function (form) {
            var formData = new FormData(form);
            $('#registerformbtn').prop("disabled", true);
            $.ajax({
                url: "/Login/RegisterUser",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    $('#registerformbtn').prop("disabled", false);
                    if(response.success == true){
                    Swal.fire({
                        icon: "success",
                        text: response.message
                          }).then((result) => {
                              if (result.isConfirmed) {
                                window.location.href = "/Login/Login";
                              }
                          });
                    }
                    if(response.success == false){
                        Swal.fire({
                          icon: "error",
                          text: response.message
                        });
                    }
                },
                error: function (res) {
                    Swal.fire({
                        icon: "error",
                        text: "Something went wrong: " + res.statusText
                    });
                }
            });
        }
    });

    $("#resetPasswordForm").validate({
        rules: {
            NewPassword: { required: true},
            ConfirmPassword: { required: true}
        },
        messages: {
            NewPassword: {
                required: "NewPassword is Required."
            },
            ConfirmPassword: {
                required: "ConfirmPassword is Required."
            }
        },
        errorElement: "div",
        errorClass: "text-danger small mt-1",
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
        },
        submitHandler: function (form) {
            $('#resetpassformsubbtn').prop("disabled", true);
            var formData = $(form);
            $.ajax({
                url: "/Login/ResetPassword",
                type: "POST",
                data: formData.serialize(),
                success: function (response) {
                    if(response.success == true){
                        window.location.href = "/Login/Login";
                    }
                    if(response.success == false){
                        Swal.fire({
                          icon: "error",
                          text: response.message
                        });
                    }
                    $('#resetpassformsubbtn').prop("disabled", false);
                },
                error: function (res) {
                    Swal.fire({
                        icon: "error",
                        text: "Something went wrong: " + res.statusText
                    });
                }
            });
        }
    });

    $("#forgotPasswordForm").validate({
        rules: {
            Email: { required: true}
        },
        messages: {
            Email: {
                required: "Email is Required."
            }
        },
        errorElement: "div",
        errorClass: "text-danger small mt-1",
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
        },
        submitHandler: function (form) {
            var formData = $(form);
            $('#forgotpassformbtn').prop("disabled", true);
            $.ajax({
                url: "/Login/ForgotPassword",
                type: "POST",
                data: formData.serialize(),
                success: function (response) {
                    $('#forgotpassformbtn').prop("disabled", false);
                    if(response.success == true){
                    window.location.href = "/Login/Login";
                    }
                    if(response.success == false){
                        Swal.fire({
                          icon: "error",
                          text: response.message
                        });
                    }
                },
                error: function (res) {
                    Swal.fire({
                        icon: "error",
                        text: "Something went wrong: " + res.statusText
                    });
                }
            });
        }
    });

    

    $("#editUserForm").validate({
        rules: {
            Username: { required: true},
            Email: { required: true},
            Password: { required: true}
        },
        messages: {
            Username: {
                required: "Username is Required."
            },
            Email: {
                required: "Email is Required."
            },
            Password: {
                required: "Password is Required."
            }
        },
        errorElement: "div",
        errorClass: "text-danger small mt-1",
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
        },
        submitHandler: function (form) {
            var formData = new FormData(form);
            $('#edituserformbtn').prop("disabled", true);
            $.ajax({
                url: "/Dashboard/EditUser",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    $('#edituserformbtn').prop("disabled", false);
                    if(response.success == true){
                    Swal.fire({
                        icon: "success",
                        text: response.message
                          }).then((result) => {
                              if (result.isConfirmed) {
                                window.location.href = "/Dashboard/Dashboard";
                              }
                          });
                    }
                    if(response.success == false){
                        Swal.fire({
                          icon: "error",
                          text: response.message
                        });
                    }
                },
                error: function (res) {
                    Swal.fire({
                        icon: "error",
                        text: "Something went wrong: " + res.statusText
                    });
                }
            });
        }
    });


    


});



function Logout() {
    $.ajax({
        url: '/Login/Logout',
        type: 'GET',
        success: function (res) {
            if(res.success == true){
                window.location.href = '/Login/Login';
                window.location.reload();
            }
        },
        error: function (res, status, error) {
            Swal.fire({
                icon: "error",
                text: "Something went wrong: " + res.statusText
            });
        }
    });
}

function ForgotPassword(email){
    Swal.fire({
      text: "Are you sure want to Reset Password ?",
      icon: "question",
      showCancelButton: true,
    }).then((result) => {
        if (result.isConfirmed) {
          $.ajax({
                url: '/Login/ForgotPassword?Email='+email,
                type: 'POST',
                success: function (res) {
                    if(res.success == true){
                        Swal.fire({
                        icon: "success",
                              text: res.message
                          }).then((result) => {
                              if (result.isConfirmed) {
                                window.location.reload();
                              }
                          });
                    }
                },
                error: function (res, status, error) {
                    Swal.fire({
                        icon: "error",
                        text: "Something went wrong: " + res.statusText
                    });
                }
            });
        }
    });
}

function previewImage() {
    const input = document.getElementById('Image');
    const preview = document.getElementById('imagePreview');
    const file = input.files[0];

    if (file) {
        const validTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
        if (!validTypes.includes(file.type)) {
            alert('Please select a valid image file (jpg, png, gif, webp).');
            input.value = ''; 
            preview.style.display = 'none';
            return;
        }

        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.display = 'block';
        };
        reader.readAsDataURL(file);
    }
    else {
        preview.src = "#";
        preview.style.display = 'none';
    }
}

function previewSelectedImage(event) {
    const input = event.target;
    const preview = document.getElementById("imagePrevieww");
    const file = input.files[0];

    if (file) {
        const validImageTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];

        if (!validImageTypes.includes(file.type)) {
            alert('Only image files (jpg, png, gif, webp) are allowed.');
            input.value = '';               
            preview.style.display = "none"; 
            return;
        }

        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.display = "block";
        };
        reader.readAsDataURL(file);
    }
}

