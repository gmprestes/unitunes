function makeid() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < 6; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}

window.oldalert = window.alert;

window.alert = function (msg) {
    if (typeof msg != 'undefined') {
        var idAlert = 'alert' + makeid();

        var text = $('<h4 />', { html: msg.toString().replace(new RegExp("\\n", "g"), '<br />') });
        text.addClass("dvPopupH4Alert");

        var fechar = $('<a />', {
            title: 'Fechar',
            onclick: 'HideModalPopup("' + idAlert + '");'
        });

        fechar.addClass('closeModal fa fa-times fa-fw');

        var modal = $('<div />', { id: idAlert });
        modal.addClass("modal");
        modal.addClass("fade");
        modal.addClass("alertModal");


        var divModal = $('<div />');
        divModal.addClass("modal-dialog");

        var divModalContent = $('<div />');

        divModalContent.addClass("modal-content");

        var divModalBody = $('<div />');
        divModalBody.addClass("modal-body");

        divModalBody.append(fechar);
        divModalBody.append(text);
        divModalContent.append(divModalBody);
        divModal.append(divModalContent);
       
        modal.append(divModal);


        $('body').append(modal);

        ShowModalPopup(idAlert);
    }
}

function dialogDecision(msg, targetId) {
    if (!window.confirm(msg)) {
        event.preventDefault(); return false;
    }
    else {
        $('#' + targetId).click();
    }
}