//Este código sirve para las validaciones del formulario para indicar la cantidad de personas en la reservación

//obtiene los valores de los datos introducidos
var adultosNacionales = document.getElementById("cantidad_Adultos_Nacional");
var ninnosNacionales = document.getElementById("cantidad_Ninnos_Nacional");
var adultosExtranjeros = document.getElementById("cantidad_Adultos_Extranjero");
var ninnosExtranjeros = document.getElementById("cantidad_ninnos_extranjero");
var form = document.getElementById("cantidad");
var err = document.getElementById("error");

const submitButton = document.getElementById('siguiente_calendario');
submitButton.addEventListener('click', validarFormulario);

function validarFormulario(event) {
    //se previene el envío del formulario hasta verificar los datos
    event.preventDefault();
    var cantidadP = parseInt(adultosNacionales.value) + parseInt(adultosExtranjeros.value) + parseInt(ninnosNacionales.value) + parseInt(ninnosExtranjeros.value);
    let messages = [];
    //si no hay personas en ninguno de los campos se indica que debe haber al menos una
    if (cantidadP < 1) {
        messages.push("Revise que haya al menos una persona");
    }
    //si la cantidad de personas excede el límite, se indica
    if (cantidadP > 40) {
        messages.push("Revise que la cantidad de personas no exceda las 40");
    }
    //si van niños debe ir un adulto responsable
    if ((ninnosNacionales.value > 0 || ninnosExtranjeros.value > 0) && (adultosNacionales.value <= 0 && adultosExtranjeros.value <= 0)) {
        messages.push("Debe ir al menos un adulto");
    }
    //se muestra el mensaje de error dependiendo del que fue captado
    if (messages.length > 0) {
        event.preventDefault();
        err.innerText = messages.join(", ");
        return false;
    }

    if ((cantidadP >= 1 && cantidadP <= 40) && (adultosNacionales.value > 0 || adultosExtranjeros.value > 0)) {
        form.submit();        
    }
}
