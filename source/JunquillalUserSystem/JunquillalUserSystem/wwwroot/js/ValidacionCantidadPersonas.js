//Este código sirve para las validaciones del formulario para indicar la cantidad de personas en la reservación

//obtiene los valores de los datos introducidos
var adultosNacionales = document.getElementById("cantidad_Adultos_Nacional");
//alert(adultosNacionales.value);
var ninnosNacionales_mayores6 = document.getElementById("cantidad_Ninnos_Nacional_mayor6");
//alert(ninnosNacionales_mayores6.value);
var ninnosNacionales_menores6 = document.getElementById("cantidad_Ninnos_Nacional_menor6");
//alert(ninnosNacionales_menores6.value);
var adulto_mayor_nacional = document.getElementById("cantidad_adulto_mayor");
//alert(adulto_mayor_nacional.value);
var adultosExtranjeros = document.getElementById("cantidad_Adultos_Extranjero");
//alert(adultosExtranjeros.value);
var ninnosExtranjeros = document.getElementById("cantidad_ninnos_extranjero");
//alert(ninnosExtranjeros.value);
var adulto_mayor_extranjero = document.getElementById("cantidad_adultoMayor_extranjero");
//alert(adulto_mayor_extranjero.value);
var form = document.getElementById("cantidad");
var err = document.getElementById("error");

const submitButton = document.getElementById('siguiente_calendario');
submitButton.addEventListener('click', validarFormulario);

function validarFormulario(event) {
    //se previene el envío del formulario hasta verificar los datos
    event.preventDefault();
    var cantidadP = parseInt(adultosNacionales.value) + parseInt(ninnosNacionales_mayores6.value) +
        parseInt(ninnosNacionales_menores6.value) + parseInt(adulto_mayor_nacional.value) +
        parseInt(adultosExtranjeros.value) + parseInt(ninnosExtranjeros.value) +
        parseInt(adulto_mayor_extranjero.value);
    let messages = [];
  
    //si no hay personas en ninguno de los campos se indica que debe haber al menos una
    if (cantidadP < 1) {
        messages.push("Revise que haya al menos una persona");
    }
    //si la cantidad de personas excede el límite, se indica
    if (cantidadP > 70) {
        messages.push("Revise que la cantidad de personas no exceda las 70 personas");
    }
    //si van niños debe ir un adulto responsable
    if ((ninnosNacionales_mayores6.value > 0 || ninnosNacionales_menores6.value > 0 || ninnosExtranjeros.value > 0)
        && (adultosNacionales.value <= 0 && adultosExtranjeros.value <= 0 && adulto_mayor_nacional.value <= 0
            && adulto_mayor_extranjero.value <= 0)) {
        messages.push("Debe ir al menos un adulto");
    }
    //se muestra el mensaje de error dependiendo del que fue captado
    if (messages.length > 0) {
        event.preventDefault();
        err.innerText = messages.join(", ");
        return false;
    }

    if ((cantidadP >= 1 && cantidadP <= 70) && (adultosNacionales.value > 0 || adultosExtranjeros.value > 0
        || adulto_mayor_nacional.value > 0 || adulto_mayor_extranjero.value > 0)) {
        form.submit();        
    }
}
