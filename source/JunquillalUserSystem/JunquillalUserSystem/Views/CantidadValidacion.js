// JavaScript source code
var adultosNacionales = document.getElementById("cantidad_Adultos_Nacional");
var ninnosNacionales = document.getElementById("cantidad_Ninnos_Nacional");
var adultosExtranjeros = document.getElementById("cantidad_Adultos_Extranjero");
var ninnosExtranjeros = document.getElementById("cantidad_ninnos_extranjero");
var form = document.getElementById("cantidad");
var err = document.getElementById("error");

form.addEventListener("submit", (e) => {
    var cantidadP = parseInt(adultosNacionales.value) + parseInt(adultosExtranjeros.value) + parseInt(ninnosNacionales.value) + parseInt(ninnosExtranjeros.value);
    let messages = [];
    if (cantidadP < 1) {
        messages.push("Revise que haya al menos una persona");
    }

    if (cantidadP > 40) {
        messages.push("Revise que la cantidad de personas no exceda las 40");
    }

    if (messages.length > 0) {
        e.preventDefault();
        err.innerText = messages.join(", ");
    }
})
