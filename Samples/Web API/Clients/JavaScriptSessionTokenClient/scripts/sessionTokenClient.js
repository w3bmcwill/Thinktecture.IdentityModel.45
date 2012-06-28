var tokenEndpoint = 'https://adfs.leastprivilege.vm/webapisecurity/api/token',
    serviceEndpoint = 'https://adfs.leastprivilege.vm/webapisecurity/api/identity',
    authN;

$(function () {
    authN = new thinktectureIdentityModel.SessionTokenAuthentication(tokenEndpoint);
});

function authenticate() {
    $('#sessionToken').val('');
    $('#data').empty();

    var un = $('#username').val(),
        pw = $('#password').val();

    authN.requestSessionToken(un, pw, tokenAvailable);
};

function tokenAvailable(sessionToken) {
    $('#sessionToken').val(sessionToken);
};

function callService() {
    authN.getLocalSessionToken(tokenEndpoint, function (token) {
        $.ajax({
            type: 'GET',
            cache: true,
            url: this.serviceEndpoint,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', authN.getSessionTokenAuthenticationHeader(token));
            },
            success: function (result) {
                $.each(result.Claims, function () {
                    $('#claims').append($('<li>').text(this.ClaimType + ':  ' + this.Value));
                });
            },
            error: function (error) {
                if (error.status == 401) {
                    alert('Unauthorized');
                }
                else {
                    alert('Error calling service: ' + error.responseText);
                }
            }
        });
    });
}