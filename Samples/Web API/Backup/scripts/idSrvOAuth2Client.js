var stsEndpoint = 'https://identity.thinktecture.com/idsrv/issue/oauth2',
    scope = 'https://samples.thinktecture.com/webapisecurity/',
    serviceEndpoint = 'https://roadie/webapi/api/identity',
    authN,
    token;

$(function () {
    authN = new thinktectureIdentityModel.BrokeredAuthentication(stsEndpoint, scope);
});

function login() {
    $('#idpToken').val('');
    $('#claims').empty();

    var un = $('#username').val(),
        pw = $('#password').val();

    authN.getIdpToken(un, pw, idpTokenAvailable);
};

function idpTokenAvailable(idpToken) {
    $('#idpToken').val(idpToken);
    token = idpToken;
};

function getIdentity() {
    $('#claims').empty();
    getIdentityClaimsFromService();
};

function getIdentityClaimsFromService() {
    var authHeader = authN.createAuthenticationHeader(token);

    $.ajax({
        type: 'GET',
        cache: false,
        url: serviceEndpoint,
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', authHeader);
        },
        success: function (result) {
            $.each(result.Claims, function (key, val) {
                $('#claims').append($('<li>').val(val.Value));
            });
        },
        error: function (error) {
            alert('Error: ' + error.responseText);
        }
    });
}