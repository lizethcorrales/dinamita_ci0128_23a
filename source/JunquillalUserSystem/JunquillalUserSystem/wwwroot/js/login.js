const divUsuario = document.querySelector("#divUsuario");
var usuario = document.getElementById("usuario");
const divContrasena = document.querySelector("#divContrasena");
var contrasena = document.getElementById("contrasena");
const cedulaValida = new RegExp("[1-9][0-9]{8}");
var botonContinuar = document.getElementById("botonContinuar");
var puesto = document.getElementById("Puesto");
var acceder = document.getElementById("guardarSesion");
var form = document.getElementById("infoUsuario");

botonContinuar.addEventListener("click", (e) => {
	var datosValido = 1;
	if (usuario.value == "") {
		divUsuario.lastElementChild.textContent = "Por favor escriba su identificación";
		datosValido = 0;
	} else {
		if (cedulaValida.test(usuario.value)) {
			divUsuario.lastElementChild.textContent = "";
		} else {
			divUsuario.lastElementChild.textContent = "Escriba un ID valido";
			datosValido = 0;
		}
	}

	if (contrasena.value == "") {
		divContrasena.lastElementChild.textContent = "Por favor escriba su contraseña";
		datosValido = 0;
	} else {
		divContrasena.lastElementChild.textContent = "";
	}
	if (datosValido > 0) {
		form.submit();
	}

});