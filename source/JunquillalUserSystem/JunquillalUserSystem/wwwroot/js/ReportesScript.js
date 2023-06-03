var divCalendarioRangoDias = document.getElementById("calendarioRangoDias");

var calendarioRangoDias = document.getElementById("visitas");
var calendarioDiario = document.getElementById("diario");
var generar = document.getElementById("btn-siguiente");
var botonLimpiar = document.getElementById("boton-limpiar");

calendarioRangoDias.addEventListener("change", (e) => {
    if (calendarioRangoDias.checked) {
        divCalendarioRangoDias.hidden = false;
    }     
});

calendarioDiario.addEventListener("change", (e) => {
    if (calendarioDiario.checked) {
        divCalendarioRangoDias.hidden = true;
    }
});

botonLimpiar.addEventListener("click", (e) => {
    limpiar();
});

function limpiar() {
    window.location.reload();
}

