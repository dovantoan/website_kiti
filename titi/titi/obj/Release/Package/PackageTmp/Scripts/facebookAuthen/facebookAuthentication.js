//$('#btnFacebookLogin').click(function () {
//    window.location.href = "/api/Account/ExternalLogin?provider=Facebook&response_type=token&client_id=self&redirect_uri=http%3a%2f%2flocalhost%3a61358%2fLogin.html&state=GerGr5JlYx4t_KpsK57GFSxVueteyBunu02xJTak5m01";

//});
var signinWin;
var APP_ID = "1501304380046711";
var REDIRECT_URI = "https://localhost:44397/";
window.fbAsyncInit = initFacebook;
var facebookLoginWindow;
var loginWindowTimer;
function initFacebook() {
    FB.init({
        appId: APP_ID,
        status: true, // check login status
        cookie: false, // enable cookies to allow the server to access the session
        xfbml: true  // parse XFBML
    });

    //FB.getLoginStatus(onFacebookLoginStatus);
    debugger;
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
};
(function () {
    var e = document.createElement('script');
    debugger;
    e.src = 'https://connect.facebook.net/en_US/all.js';
    e.async = true;
    document.getElementById('fb-root').appendChild(e);
}());
function CheckLoginStatus() {
    if (signinWin.closed) {
        $('#UserInfo').text($.cookie("some_cookie"));
    }
    else setTimeout(CheckLoginStatus, 1000);
}

function _facebookLogin() {
    var popupWidth = 500;
    var popupHeight = 300;
    var xPosition = ($(window).width() - popupWidth) / 2;
    var yPosition = ($(window).height() - popupHeight) / 2;
    var loginUrl = "http://www.facebook.com/dialog/oauth/?" +
        "scope=public_profile,email&" +
        "client_id=" + APP_ID+"&" +
        "redirect_uri=" + REDIRECT_URI+"&" +
        "response_type=token&" +
        "display=popup";

    facebookLoginWindow = window.open(loginUrl, "LoginWindow",
        "location=1,scrollbars=1," +
        "width=" + popupWidth + ",height=" + popupHeight + "," +
        "left=" + xPosition + ",top=" + yPosition);

    loginWindowTimer = setInterval(onTimerCallbackToCheckLoginWindowClosure, 1000);
}
function onTimerCallbackToCheckLoginWindowClosure() {
    // If the window is closed, then reinit Facebook
    if (facebookLoginWindow.closed) {
        clearInterval(loginWindowTimer);
        FB.init({
            appId: APP_ID,
            status: true, // check login status
            cookie: true, // enable cookies to allow the server to access the session
            xfbml: true  // parse XFBML
            
        });

        FB.getLoginStatus(onFacebookLoginStatus);
    }
}
function onFacebookLoginStatus(response) {
    alert("onFacebookLoginStatus response.status=" + response.status + " response.session=" + response.session);
    debugger;
    if (response.status == "connected" && response.authResponse) {
        var loginButtonDiv = document.getElementById("fb-login-button-div");
        loginButtonDiv.style.display = "none";
        var logoutButtonDiv = document.getElementById("fb-logout-button-div");
        logoutButtonDiv.style.display = "block";
        var contentDiv = document.getElementById("user-is-authenticated-div");
        contentDiv.style.display = "block";
        FB.api("/me", onMyInfoLoaded);
    }
    else {
        alert('chưa login');
        //var loginButtonDiv = document.getElementById("fb-login-button-div");
        //loginButtonDiv.style.display = "block";
        //var contentDiv = document.getElementById("user-is-authenticated-div");
        //contentDiv.style.display = "none";
    }
    function onMyInfoLoaded(response) {
        var contentDiv = document.getElementById("user-is-authenticated-div");
        contentDiv.innerHTML = "<h1>Welcome " + response.name + ", you are logged in.</h1>";
    }
} 
function statusChangeCallback(response) {  // Called with the results from FB.getLoginStatus().
    console.log('statusChangeCallback');
    console.log(response);                   // The current login status of the person.
    if (response.status === 'connected') {   // Logged into your webpage and Facebook.
        testAPI();
    } else {                                 // Not logged into your webpage or we are unable to tell.
        document.getElementById('status').innerHTML = 'Please log ' +
            'into this webpage.';
    }
}
function testAPI() {                      // Testing Graph API after login.  See statusChangeCallback() for when this call is made.
    console.log('Welcome!  Fetching your information.... ');
    FB.api('/me', function (response) {
        console.log('Successful login for: ' + response.name);
        debugger;
        document.getElementById('status').innerHTML =
            'Thanks for logging in, ' + response.name + '!';
    });
}