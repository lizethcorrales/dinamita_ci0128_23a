var fechaEntrada = document.getElementById("fecha-entrada");
var campos = document.getElementById("campos");
var divCamposDisponibles = document.querySelector("#camposDisponibles");
var formCampos = document.getElementById("CamposDisponiblesForm");

console.log(divCamposDisponibles.childElementCount);


buscar.addEventListener("click", (e) => {
    if (fechaEntrada.value != "") {
        formCampos.submit();
        divCamposDisponibles.lastElementChild.textContent = "";
    } else {
        divCamposDisponibles.lastElementChild.textContent = "Debe de ingresar una fecha válida";
    }
});