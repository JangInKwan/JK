/// 동작 없음
function None() {

}

/// 리로드
function Reload() {
    location.reload();
}

/// 페이지 이동
function LocationLink(link) {
    location.href = link;
}

/// 팝업
var Popup = {
    Show: function (msg, fc) {
        $("#bntAlert").off("click");

        if (fc != undefined) {
            $('#bntAlert').click(fc);
        }

        $('#txtAlertMsg').html(msg);
        $('#popAlert').modal('show');

        //alert(msg);
        //eval(fc);
    },
    Close: function () {
        $('#popAlert').modal('hide');
    }
}

/// Confirm
var Confirm = {
    Show: function (msg, fcOK, fcCancel) {
        $("#bntConfirmOK").off("click");
        $("#bntConfirmClose").off("click");

        if (fcOK != undefined) {
            $('#bntConfirmOK').click(fcOK);
        }

        if (fcCancel != undefined) {
            $('#bntConfirmClose').click(fcCancel);
        }

        $('#txtConfirmMsg').html(msg);
        $('#popConfirm').modal('show');
    },
    Close: function () {
        $('#popConfirm').modal('hide');
    }
}

/// Empty 확인
function IsEmpty(val) {
    if (val == undefined || val == null || val == '' || val.trim() == '') {
        return true;
    }
    else {
        return false;
    }
}

/// Validation 체크
var Validation = {
    Empty: function (val, msg, span) {
        if (!IsEmpty(val)) {
            msg = '';
        }

        if (IsEmpty(msg)) {
            return true;
        }
        else {
            if (IsEmpty(span)) {
                Popup.Show(msg);
            }
            else {
                $('#' + span).html(msg);
            }

            return false;
        }
    },
    Number: function (val, msg, span) {
        if (!(/^[0-9]+$/g.test(val))) {
            msg = "숫자만 사용가능합니다.";
        }

        if (IsEmpty(msg)) {
            return true;
        }
        else {
            if (IsEmpty(span)) {
                Popup.Show(msg);
            }
            else {
                $('#' + span).html(msg);
            }

            return false;
        }
    },

    MinLengthID: 4,
    MaxLengthID: 20,
    ID: function (val, span) {
        var msg = '';

        if (IsEmpty(val)) {
            msg = "아이디를 입력해야 합니다.";
        }
        else if (val.length < this.MinLengthID) {
            msg = "아이디를 " + this.MinLengthID + "자 이상이여야 합니다.";
        }
        else if (val.length > this.MaxLengthID) {
            msg = "아이디를 " + this.MaxLengthID + "자 이하이여야 합니다.";
        }
        else if (!(/^[A-Za-z0-9]+$/g.test(val))) {
            msg = "아이디는 영문, 숫자만 사용가능합니다.";
        }

        if (IsEmpty(msg)) {
            return true;
        }
        else {
            if (IsEmpty(span)) {
                Popup.Show(msg);
            }
            else {
                $('#' + span).html(msg);
            }

            return false;
        }
    },

    MinLengthPassword: 4,
    MaxLengthPassword: 20,
    Password: function (val, span) {
        var msg = '';

        if (IsEmpty(val)) {
            msg = "비밀번호를 입력해야 합니다.";
        }
        else if (val.length < this.MinLengthPassword) {
            msg = "비밀번호는 " + this.MinLengthPassword + "자 이상이여야 합니다.";
        }
        else if (val.length > this.MaxLengthPassword) {
            msg = "비밀번호는 " + this.MaxLengthPassword + "자 이하이여야 합니다.";
        }
        else if (!(/^[A-Za-z0-9~`!@#$%^&*()_\-+={}[\]|\\;:'""<>,.?/]+$/g.test(val))) {
            msg = "아이디는 영문, 숫자, 특수문자만 사용가능합니다.";
        }

        if (IsEmpty(msg)) {
            return true;
        }
        else {
            if (IsEmpty(span)) {
                Popup.Show(msg);
            }
            else {
                $('#' + span).html(msg);
            }

            return false;
        }
    }
};

/// Validation 체크
var Keypress = {
    OnlyNumber: function() {
        if ((event.keyCode < 48) || (event.keyCode > 57))
            event.returnValue = false;
    }
};

/// Enter key 입력 이벤트 
var KeypressEnterKey = {
    ElementFocus: function (id) {
        if (event.keyCode == 13)
            $('#' + id).focus();
    },
    Eval: function (fc) {
        if (event.keyCode == 13)
            eval(fc);
    }
};

function FillZero(width, val) {
    var str = val.value;
    val.value =  str.length >= width ? str : new Array(width - str.length + 1).join('0') + str;
}


/// 전체화면 오픈
function WindowFullScreenOpen(url, title) {
    //window.open(url, title, 'height=' + screen.height + ',width=' + screen.width + 'fullscreen=yes');
    window.open(url, title);
}
