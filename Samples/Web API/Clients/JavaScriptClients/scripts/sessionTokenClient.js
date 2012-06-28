var tokenEndpoint = 'https://adfs.leastprivilege.vm/webapisecurity/api/token',
    serviceEndpoint = 'https://adfs.leastprivilege.vm/webapisecurity/api/identity',
    authN,
    isExpired;

$(function () {
    authN = new thinktectureIdentityModel.SessionTokenAuthentication(tokenEndpoint);

    authN.getLocalSessionToken(tokenEndpoint, function (token) {
        if (token == null) {
            return;
        };

        isExpired = isTokenExpired(token);

        if (isExpired == true) {
            $('#sessionToken').val('');
        }
        else {
            $('#sessionToken').val(token.access_token);
        }
    });
});

function authenticate() {
    $('#sessionToken').val('');
    $('#claims').empty();

    var un = $('#username').val(),
        pw = $('#password').val();

    authN.requestSessionToken(un, pw, tokenAvailable);
};

function tokenAvailable(sessionToken) {
    $('#sessionToken').val(sessionToken.access_token);
    isExpired = false;
};

function clearTokenCache() {
    authN.clearTokenCache();
    $('#sessionToken').val('');
};

function isTokenExpired(token) {
    var nowEpoch = Math.round(new Date().getTime() / 1000.0);

    return token.expires_in - nowEpoch < 0;
};

function callService() {
    $('#claims').empty();

    authN.getLocalSessionToken(tokenEndpoint, function (token) {
        if (token == null) {
            alert('No token available');
            return;
        };

        isExpired = isTokenExpired(token);

        if (isExpired == true) {
            alert('Token is expired!');
        }
        else {
            $.ajax({
                type: 'GET',
                cache: false,
                url: this.serviceEndpoint,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', authN.getSessionTokenAuthenticationHeader(token.access_token));
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
        }
    });
}