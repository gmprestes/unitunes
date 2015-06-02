function downloadFile(requestID, fileUrl) {
    jQuery('<iframe id="' + requestID + '"  src="' + fileUrl + '"></iframe>').appendTo('body').hide();
    setTimeout(function () { $('#' + requestID).remove(); }, 30000);
}
