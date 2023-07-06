var apiUrl = "https://localhost:44348/";
var pathname = window.location.pathname;
var language = "tr-TR";
var pageNumber = 1;
var pageSize = 20;

//Check Storage For Token
function getAccessToken() {
    var loginDataJson = sessionStorage["login"] || localStorage["login"];
    var loginData;
    try {
        loginData = JSON.parse(loginDataJson);
    }
    catch (error) {
        return null;
    }
    if (!loginData) {
        return null;
    }
    return loginData;
};

function getAuthHeaders() {
    return { Authorization: "Bearer " + getAccessToken() };
};

//Checking Authorized User
function checkAuth() {
    if (pathname.endsWith("/login.html")) {
        return;
    }

    // Get Token If Exists
    var accessToken = getAccessToken();
    if (!accessToken) {

        window.location.href = "login.html";
        return;
    }
    changeLanguage(localStorage["language"]);

    // Verify Token
    $.ajax({
        type: "get",
        headers: getAuthHeaders(),
        url: apiUrl + "api/Authorize/verifyToken",
        success: function (data, xhr) {

            loadData();
            notification('info', data.message + ' ' + data.username);
        },
        error: function (xhr, error, status) {
            pageNumber = 1;
            window.location.href = "login.html";
        }
    });

};

//Login
$("#loginForm").submit(function (event) {
    event.preventDefault();
    var remember = $("#rememberMe").prop("checked"); // true | false
    $.ajax({
        type: "POST",
        url: apiUrl + "api/Authorize/login",
        dataType: "json",
        data: $(this).serialize(),
        success: function (data) {
            localStorage.removeItem("login");
            sessionStorage.removeItem("login");
            var storage = remember ? localStorage : sessionStorage;
            storage["login"] = JSON.stringify(data.token);
            location.href = "index.html";
        },
        error: function (xhr, status, error) {
            notification("error", xhr.responseJSON.Message);
            $("#loginPassword").val("");
        },
    });
});

$("#btnRegisterForm").click(function () {
    $("#registerForm").trigger("reset");
});

//Register
$("#registerForm").submit(function (event) {
    event.preventDefault();
    $.ajax({
        type: "POST",
        url: apiUrl + "api/Authorize/Register",
        data: $(this).serialize(),
        success: function (data) {
            $("#loginUsername").val($("#registerUsername").val());
            $("#loginPassword").val($("#registerPassword").val());
            $("#loginForm").trigger("submit");
        },

        error: function (xhr, status, error) {
            notification('error', xhr.responseJSON.Message);
        }
    });

});

// Loading effect
$(document).ajaxStart(function () {
    $("#loading").removeClass("d-none");
});
$(document).ajaxStop(function () {
    $("#loading").addClass("d-none");
});

//File Upload
$("#importForm").submit(function (event) {
    var fileData = new FormData();
    var files = $("#customFile").get(0).files;

    if (files.length > 0) {
        fileData.append("formFile", files[0]);
    }
    event.preventDefault();
    $.ajax({
        type: "POST",
        headers: getAuthHeaders(),
        url: apiUrl + "api/Data",
        contentType: false,
        processData: false,
        data: fileData,
        success: function (data) {
            notification('success', data);
            loadData();
        },
        error: function (xhr, status, error) {
            notification('error', xhr.responseJSON.Message);
        },
    });
});

//Notification
function notification(type, message) {
    var Toast = Swal.mixin({
        toast: true,
        position: 'top',
        showConfirmButton: false,
        timer: 3000
    });
    Toast.fire({
        icon: type,
        title: message
    })
};

//Password eyes
function showLoginPassword() {
    var x = document.getElementById("loginPassword");
    var y = document.getElementById("loginEye");
    changeIcon(x, y);

};

function showRegisterPassword() {
    var x = document.getElementById("registerPassword");
    var y = document.getElementById("registerEye");
    changeIcon(x, y);
};

function changeIcon(x, y) {
    if (x.type === "password") {
        x.type = "text";
        y.classList.add("fa-eye-slash");
        y.classList.remove("fa-eye");
    } else {
        x.type = "password";
        y.classList.remove("fa-eye-slash");
        y.classList.add("fa-eye");
    }
};

//Input File
$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});

//Get Data
function loadData() {

    var jqxhr = $.ajax({
        type: "get",
        headers: getAuthHeaders(),
        url: apiUrl + "api/Data",
        data: {
            "pageNumber": pageNumber,
            "pageSize": pageSize,
        },
        success: function (data, xhr, request) {
            listData(data, jqxhr.getResponseHeader("X-Pagination"));
        },
        error: function (xhr, error, status) {
            console.log(xhr.responseJSON);
        }
    });
}

function listData(data, paginationValues) {
    const paginationObj = JSON.parse(paginationValues);
    pageNumber = paginationObj.CurrentPage;
    $("#main-table-body").empty();

    $.each(data, function (key, value) {
        $("#main-table-body").append('<tr class="clickable-row">' +
            '<td>' + value.date + '</td>' +
            '<td>' + value.category + '</td>' +
            '<td>' + value.event + '</td>' +
            '</tr>');
    });

    $("#pageNumber").text('(' + pageNumber + ')');
    console.log(paginationObj);
    if (paginationObj.HasNext) {
        $("#nextBtn").removeAttr('disabled');
    }
    else {
        $("#nextBtn").prop("disabled", true);
    }

    if (paginationObj.HasPrevious) {
        $("#prevBtn").removeAttr('disabled');
    }
    else {
        $("#prevBtn").prop("disabled", true);
    }
}

//Next Data
function nextData() {
    pageNumber++;
    loadData();

}

//Previous Data
function prevData() {
    pageNumber--;
    loadData();
}

//LogOut
function logOut() {
    localStorage.removeItem("language");
    localStorage.removeItem("login");
    sessionStorage.removeItem("login");
    window.location.href = "login.html";
};

//Get Language Setting Files
function changeLanguage(lang) {
    language = lang;
    if (lang == "it-IT") {
        localStorage["language"] = "it-IT";
        $.ajax({
            type: "get",
            url: apiUrl + "api/Setting",
            dataType: "json",
            success: function (data, xhr, request) {
                console.log(data);
                localizeUI(data);
            },
            error: function (xhr, error, status) {
                console.log(xhr.responseJSON);
            }
        });
    }
    else {
        localStorage["language"] = "tr-TR";
        $.ajax({
            type: "get",
            url: apiUrl + "api/Setting",
            dataType: "json",
            success: function (data, xhr, request) {
                console.log(data);
                localizeUI(data);
            },
            error: function (xhr, error, status) {
                console.log(xhr.responseJSON);
            }
        });
    }
}

function localizeUI(data) {
    var registerText = data.filter(obj => { return obj.key === 'Register' });
    var loginText = data.filter(obj => { return obj.key === 'Login' });
    var rememberMeText = data.filter(obj => { return obj.key === 'RememberMe' });
    var uploadFileText = data.filter(obj => { return obj.key === 'UploadFile' });
    var chooseFileText = data.filter(obj => { return obj.key === 'ChooseFile' });
    var uploadText = data.filter(obj => { return obj.key === 'Upload' });
    var prevBtnText = data.filter(obj => { return obj.key === 'Prev' });
    var nextBtnText = data.filter(obj => { return obj.key === 'Next' });
    var dateText = data.filter(obj => { return obj.key === 'Date' });
    var categoryText = data.filter(obj => { return obj.key === 'Category' });
    var eventText = data.filter(obj => { return obj.key === 'Event' });
    var usernameText = data.filter(obj => { return obj.key === 'Username' });
    var passwordText = data.filter(obj => { return obj.key === 'Password' });
    $("#registerText").text(registerText[0].value);
    $("#loginText").text(loginText[0].value);
    $("#registerBtnText").text(registerText[0].value);
    $("#rememberMeText").text(rememberMeText[0].value);
    $("#loginBtnText").text(loginText[0].value);
    $("#uploadFileText").text(uploadFileText[0].value);
    $("#chooseFileText").text(chooseFileText[0].value);
    $("#uploadText").text(uploadText[0].value);
    $("#prevBtnText").text(prevBtnText[0].value);
    $("#nextBtnText").text(nextBtnText[0].value);
    $("#dateText").text(dateText[0].value);
    $("#categoryText").text(categoryText[0].value);
    $("#eventText").text(eventText[0].value);
    $("#loginUsername").attr('placeholder', usernameText[0].value);
    $("#loginPassword").attr('placeholder', passwordText[0].value);
    $("#registerUsername").attr('placeholder', usernameText[0].value);
    $("#registerPassword").attr('placeholder', passwordText[0].value);
}

//Localization Header
$(document).ajaxSend(function (event, jqxhr, settings) {
    jqxhr.setRequestHeader('Accept-Language', language);
});

checkAuth();
