var fechaEntrada = document.getElementById("fecha-entrada");
var campos = document.getElementById("campos");
var divCamposDisponibles = document.querySelector("#camposDisponibles");

console.log(divCamposDisponibles.childElementCount);

buscar.addEventListener("click", (e) => {
    if (fechaEntrada.value != "") {
        campos.value = "30";
        divCamposDisponibles.lastElementChild.textContent = "";
    } else {
        divCamposDisponibles.lastElementChild.textContent = "Debe de ingresar una fecha válida";
    }
});