
function showError(title,txt) {
    $("#errorTitle").html(title);
    $("#errorText").html(txt);
    $("#errorModal").modal();    
}


function showInfo(title,txt) {
    $("#infoTitle").html(title);
    $("#infoText").html(txt);
    $("#infoModal").modal();
}
