function ChangePwd() {
    ExecuteOrDelayUntilScriptLoaded(function () {
        var options = SP.UI.$create_DialogOptions();
        options.title = "";
        options.width = 510;
        options.height = 260;
        options.url = "/Pages/ChangePassword/ChangePassword.aspx";
        //options.dialogReturnValueCallback = Function.createDelegate(null, UpdateRightContent);
        SP.UI.ModalDialog.showModalDialog(options);
    }, 'sp.js');
}

function Reset()
{
    layer.load("保存中,请稍后...");

    $(".Reset").click();
}

function closePage()
{
    window.frameElement.cancelPopUp();
}

function ResetSuccess() {
    var index = $.layer({
        area: ['auto', 'auto'],
        dialog: {
            msg: '密码重置成功。',
            type: 9
        },
        btns: 1,
        btn: ['确定'],
        closeBtn: false,
        yes: function () {
            closePage();
            layer.close(index);
        }
    });
}


function ResetFailed(msg) {
    var index = $.layer({
        area: ['auto', 'auto'],
        dialog: {
            msg: msg,
            type: 8
        },
        btns: 1,
        btn: ['确定'],
        closeBtn: false,
        yes: function () {
            layer.close(index);
        }
    });
}

/* Frank add login with another user and signout function --- 2015-10-13 */
function loginwithanotheruser() {
    window.location = "/_layouts/15/closeConnection.aspx?loginasanotheruser=true";
}

function signout() {
    window.location = "/_layouts/15/closeConnection.aspx";
    window.location = "/_layouts/15/SignOut.aspx";
    window.close();
}