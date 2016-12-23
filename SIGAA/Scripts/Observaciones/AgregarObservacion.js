
$("#btnAgregarObservacion").click(function (eve) {
    $("#modal-content").load("/EGRE/RecepcionesTFG/CrearDetalleRecepcionTFG");
});

//definida en el OptionAjax del Ajax Begin Form
function Success(data) {
    $("#sObsCorta").val("");
    $("#sObsDetallada").val("");
    $("#sSugerencias").val("");
    $("#sTipoObs_fl").focus();
    //$("#AgregarObservacionTFG").attr('disabled', true);
}

function eliminarFila(index) {
    $("#fila_" + index).remove();
}

$(".btnEditar").click(function (eve) {
    $("#modal-content").load("/EGRE/RecepcionesTFG/EliminiarDetalleRecepcionTFG" + $(this).data("id"));
});


