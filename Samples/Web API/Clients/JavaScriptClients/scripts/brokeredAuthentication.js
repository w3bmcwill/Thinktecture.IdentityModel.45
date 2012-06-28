thinktectureIdentityModel.BrokeredAuthentication = function (stsEndpointAddress, scope) {
    that = this;
    that.stsEndpointAddress = stsEndpointAddress;
    that.scope = scope;
};

thinktectureIdentityModel.BrokeredAuthentication.prototype = function () {
    requestIdpToken = function (un, pw, callback) {
        $.ajax({
            type: 'POST',
            cache: false,
            url: that.stsEndpointAddress,
            data: { grant_type: "password", username: un, password: pw, scope: that.scope },
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
        requestIdpToken: requestIdpToken,
        createAuthenticationHeader: createAuthenticationHeader
    };
} ();