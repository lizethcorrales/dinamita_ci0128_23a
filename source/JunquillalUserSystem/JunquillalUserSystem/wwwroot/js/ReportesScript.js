var divCalendarioRangoDias = document.getElementById("calendarioRangoDias");

var calendarioRangoDias = document.getElementById("visitas");
var calendarioDiario = document.getElementById("diario");
var generar = document.getElementById("btn-siguiente");

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


