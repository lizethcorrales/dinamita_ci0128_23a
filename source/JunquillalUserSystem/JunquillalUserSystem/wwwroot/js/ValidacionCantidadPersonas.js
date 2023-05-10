// JavaScript source code
var adultosNacionales = document.getElementById("cantidad_Adultos_Nacional");
var ninnosNacionales = document.getElementById("cantidad_Ninnos_Nacional");
var adultosExtranjeros = document.getElementById("cantidad_Adultos_Extranjero");
var ninnosExtranjeros = document.getElementById("cantidad_ninnos_extranjero");
var form = document.getElementById("cantidad");
var err = document.getElementById("error");

const submitButton = document.getElementById('siguiente_calendario');
submitButton.addEventListener('click', validarFormulario);

function validarFormulario(event) {
    event.preventDefault();
    var cantidadP = parseInt(adultosNacionales.value) + parseInt(adultosExtranjeros.value) + parseInt(ninnosNacionales.value) + parseInt(ninnosExtranjeros.value);
    let messages = [];

    if (cantidadP < 1) {
        messages.push("Revise que haya al menos una persona");
    }

    if (cantidadP > 40) {
        messages.push("Revise que la cantidad de personas no exceda las 40");
    }

    if ((ninnosNacionales.value > 0 || ninnosExtranjeros.value > 0) && (adultosNacionales.value <= 0 && adultosExtranjeros.value <= 0)) {
        messages.push("Debe ir al menos un adulto");
    }

    if (messages.length > 0) {
        event.preventDefault();
        err.innerText = messages.join(", ");
        return false;
    }

    if ((cantidadP >= 1 && cantidadP <= 40) && (adultosNacionales.value > 0 || adultosExtranjeros.value > 0)) {
        form.submit();        
    }
}
