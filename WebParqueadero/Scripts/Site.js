function InicioSesionCargando() {
    var Correo = $("#Email").val();
    var Contraseña = $("#Password").val();
    if (Correo != "" && Contraseña != "") {
        var cargando = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Iniciando...`;
        $("button").attr("disabled");
        $("button").html(cargando);
    }
}

function RegistrarCargando() {
    var NombreEmpresa = $("#NombreEmpresa_Parq").val();
    var NitEmpresa = $("#NitEmpresa_Parq").val();
    var Telefono = $("#Telefono_Parq").val();
    var Direccion = $("#Direccion_Parq").val();
    var Correo = $("#Correo_Parq").val();
    var Contraseña = $("#CorreoContra_Parq").val();
    var HoraApertura = $("#HoraApertura_Parq").val();
    var HoraCierre = $("#HoraCierre_Parq").val();
    var PagoMinutos = $("#PagoMinutos_Parq").val();
    var Valor = $("#Valor_Parq").val();

    if (Correo != "" && Contraseña != "" && NombreEmpresa != "" && NitEmpresa != "" && Telefono != "" && Direccion != "" && PagoMinutos != "" && Valor != "") {
        var cargando = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Registrando...`;
        $("button").attr("disabled");
        $("button").html(cargando);
    }
}