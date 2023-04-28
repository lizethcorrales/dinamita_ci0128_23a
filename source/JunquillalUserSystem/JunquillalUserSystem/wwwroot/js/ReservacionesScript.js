// location.href='../Home/DatosReserva'"
const divNombre = document.querySelector("#divNombre");
var nombre = document.getElementById("nombre");
console.log(divNombre.lastElementChild);


continuar.addEventListener("click", (e) => {
	if (nombre.value.length < 1) {

		divNombre.lastElementChild.textContent = "Nombre NO válido";
		console.log(divNombre.lastElementChild);
	}

});