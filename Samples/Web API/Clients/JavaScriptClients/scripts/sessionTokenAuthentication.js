thinktectureIdentityModel.SessionTokenAuthentication = function (endpoint) {
    that = this;
    that.endpointAddress = endpoint;
    that.store = new Lawnchair({ name: 'tt.idm.sessionToken', adapter: 'dom' }, function () {
    });
};

thinktectureIdentityModel.SessionTokenAuthentication.prototype = function () {
    requestSessionToken = function (un, pw, callback) {
        $.ajax({
            type: 'GET',
            cache: false,
            url: that.endpointAddress,
            beforeSend: function (xhr) { 
                xhr.setRequestHeader('Authorization', createBasicAuthenticationHeader(un, pw)); 
            },
            success: function (result) {                
                that.store.save({ key: that.endpointAddress, sessionToken: result }, function (obj) {
                });

                callback(result);
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
    };

    createBasicAuthenticationHeader = function (un, pw) {
        var header = 'Basic ' + $.base64.encode(un + ":" + pw);

        return header;
    };

    getSessionTokenAuthenticationHeader = function (token) {
        var header = 'Session ' + token;

        return header;
    };

    getLocalSessionToken = function (key, callback) {
        that.store.get(key, function (tokenEntry) {
            if (tokenEntry != null) {
                callback(tokenEntry.sessionToken);
            }
            else {
                callback(null);
            }
        });        
    };
    
    clearTokenCache = function (key) {
        that.store.nuke();
    };

    return {
        requestSessionToken: requestSessionToken,
        getSessionTokenAuthenticationHeader: getSessionTokenAuthenticationHeader,
        getLocalSessionToken: getLocalSessionToken,
        clearTokenCache: clearTokenCache
    };
}();