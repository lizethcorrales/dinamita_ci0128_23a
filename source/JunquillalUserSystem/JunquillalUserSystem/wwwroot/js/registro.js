const divUsuario = document.querySelector("#divUsuarioNuevo");
var nombre = document.getElementById("nombre");

const divPrimerApellido = document.querySelector("#divUsuarioApellidoP");
var primerApellido = document.getElementById("papellido");

const divSegundoApellido = document.querySelector("#divUsuarioApellidoS");
var segundoApellido = document.getElementById("sapellido");

const divCedula = document.querySelector("#divCedula");
var cedula = document.getElementById("cedula");

const divCorreo = document.querySelector("#divCorreo");
var correo = document.getElementById("correo");

const divContrasena = document.querySelector("#divContrasena");
var contrasena = document.getElementById("contrasena");

const divContrasenaDN = document.querySelector("#divContrasenaDN");
var contrasenaDN = document.getElementById("contrasenaDN");

const divPuesto = document.querySelector("#divPuesto");
var puesto = document.getElementById("puesto");

var botonRegistrar = document.getElementById("botonRegistrar");

const divError = document.querySelector("#divError");

const cedulaValida = new RegExp("[1-9][0-9]{8}");
const correoValido = new RegExp("[a-z0-9A-Z]+\@[a-z]+\.(com|net)");

var form = document.getElementById("infoUsuarioNuevo");

botonRegistrar.addEventListener("click", (e) => {
	var datosValido = 1;
	//e.preventDefault();
	divError.lastElementChild.textContent = "";
	
	if (nombre.value == "" || primerApellido.value == "") {
		datosValido = -1;
	}

	if (segundoApellido.value == "") {
		segundoApellido.value = "NA";
	}

	if (cedula.value == "") {
		datosValido = -1;
	} else {
		if (cedulaValida.test(cedula.value)) {
			divCedula.lastElementChild.textContent = "";
		} else {
			divCedula.lastElementChild.textContent = "Escriba una cedula valida";
			datosValido = 0;
		}
	}

	if (correo.value == "") {
		datosValido = -1;
	} else {
		if (correoValido.test(correo.value)) {
			divCorreo.lastElementChild.textContent = "";
		} else {
			divCorreo.lastElementChild.textContent = "Escriba un correo valido";
			datosValido = 0;
		}
	}

	if (contrasena.value == "" || contrasenaDN.value == "") {
		datosValido = -1;		
	} else {
		if (contrasena.value != contrasenaDN.value) {
			divError.lastElementChild.textContent = "Las contraseñas no coinciden";
			datosValido = 0;
		}
	}

	if (datosValido > 0) {
		form.submit();
	} else if (datosValido == -1) {
		divError.lastElementChild.textContent = "Llene todos los datos del usuario";
	}

});