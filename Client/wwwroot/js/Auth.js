/*$(document).ready(function () {
    
    $('#loginForm').submit(function (event) {
        //debugger;
        event.preventDefault();
        var login = new Object
        //debugger;
        login.email = $('#email').val();
        login.password = $('#password').val();
        $.ajax({
            url: 'http://localhost:8042/api/Accounts/Login',
            type: 'POST',
            data: JSON.stringify(login),
            contentType: "application/json; charset=utf-8",
            dataType :"json",
            success: function (result) {
                
                $.post("/Auth/Login", { email: login.email })
                    .done(function () {
                        Swal.fire({
                            title: "Success!",
                            showConfirmButton: false,
                            timer: 1000,
                            icon: "success",
                            showConfirmButton: false,
                           
                        })
                            .then(successAlert => {
                                if (successAlert) {
                                    location.replace("/Departments/Index")
                                }
                              
                            })
                    })
                
            },
            error: function (error) {
                $('#errorMessage').show();
                setTimeout(function () {
                    $('#errorMessage').hide();
                }, 5000); 
            }
        });
    });
});*/

/*
function Session(email) {
    debugger;
    $.ajax({
        type: 'POST',
        url: 'Departments/Index',
        data: { email : email },
        success: function (result) {
          
            console.log("SUKSES");
          
        },
        error: function () {
            console.log('Session get failed');
        }
    });
}*/

$(document).ready(function () {

    $('#loginForm').submit(function (event) {
        //debugger;
        event.preventDefault();
        var login = new Object
        debugger;
        login.email = $('#email').val();
        login.password = $('#password').val();
        $.ajax({
            url: 'http://localhost:8042/api/Jwt/Login',
            type: 'POST',
            data: JSON.stringify(login), //Kirim data login
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
  
                var token = result.token;
                sessionStorage.setItem("tokenJWT",token);

                $.post("/Auth/Login", {token })
                    .done(function () {
                    
                        Swal.fire({
                            title: "Success!",
                            showConfirmButton: false,
                            timer: 1000,
                            icon: "success",
                            showConfirmButton: false,

                        })
                            .then(successAlert => {
                                if (successAlert) {
                                    location.replace("/Charts/Index")
                                }

                            })
                    })

            },
            error: function (error) {
                $('#errorMessage').show();
                setTimeout(function () {
                    $('#errorMessage').hide();
                }, 5000);
            }
        });
    });
});

function Logout() {
        sessionStorage.removeItem('tokenJWT'); //Remove Session
        window.location.href = 'https://localhost:7005'; //Kembali Ke halaman Awal 
  
}