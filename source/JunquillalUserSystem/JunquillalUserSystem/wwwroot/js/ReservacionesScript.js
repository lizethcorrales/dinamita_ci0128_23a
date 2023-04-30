// location.href='../Home/DatosReserva'"
const divNombre = document.querySelector("#divNombre");
var nombre = document.getElementById("nombre");

const divApellido1 = document.querySelector("#divApellido1");
console.log(document.getElementById("nombre").value);
const primerApellido = document.getElementById("primerApellido");

const divApellido2= document.querySelector("#divApellido2");
var segundoApellido = document.getElementById("segundoApellido");

const divIdentificacion = document.querySelector("#divIdent");
var identificacion = document.getElementById("identificacion");



continuar.addEventListener("click", (e) => {
	if (nombre.value.length < 1) {
		divNombre.lastElementChild.textContent = "Nombre NO válido";
	}
	console.log(primerApellido.value);
	if (primerApellido.value.length < 1) {	
		divApellido1.lastElementChild.textContent = "Apellido NO válido";
	}
	if (segundoApellido.value.length < 1) {
		divApellido2.lastElementChild.textContent = "Apellido NO válido";
	}
	if (identificacion.value.length < 1) {
		divIdentificacion.lastElementChild.textContent = "Identificación NO válido";
	}

});