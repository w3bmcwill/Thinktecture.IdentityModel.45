var thinktectureIdentityModel = {};

thinktectureIdentityModel.BrokeredAuthentication = function (stsEndpointAddress, scope) {
    this.stsEndpointAddress = stsEndpointAddress;
    this.scope = scope;
};

thinktectureIdentityModel.BrokeredAuthentication.prototype = function () {
    getIdpToken = function (un, pw, callback) {
        $.ajax({
            type: 'POST',
            cache: false,
            url: this.stsEndpointAddress,
            data: { grant_type: "password", username: un, password: pw, scope: this.scope },
            success: function (result) {
                callback(result.access_token);
            },
            error: function (error) {
                if (error.status == 401) {
                    alert('Unauthorized');
                }
                else {
                    alert('Error calling STS: ' + error.responseText);
                }
            }
        });
    };

    createAuthenticationHeader = function (token) {
        var tok = 'IdSrv ' + token;

        return tok;
    };

    return {
        getIdpToken: getIdpToken,
        createAuthenticationHeader: createAuthenticationHeader
    };
} ();