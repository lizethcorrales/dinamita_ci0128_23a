const divNombre = document.querySelector("#divNombre");
var nombre = document.getElementById("nombre");

const divApellido1 = document.querySelector("#divApellido1");
console.log(document.getElementById("nombre").value);
const primerApellido = document.getElementById("primerApellido");

const divApellido2 = document.querySelector("#divApellido2");
var segundoApellido = document.getElementById("segundoApellido");

const divIdentificacion = document.querySelector("#divIdent");
var identificacion = document.getElementById("identificacion");

var vehiculo = document.getElementById("vehiculo");
const divPlaca1 = document.querySelector("#divPlaca1");
const divPlaca2 = document.querySelector("#divPlaca2");
const divPlaca3 = document.querySelector("#divPlaca3");
const divPlaca4 = document.querySelector("#divPlaca4");
var continuar = document.getElementById("continuar");

var placa1 = document.getElementById("placa1");
var placa2 = document.getElementById("placa2");
var placa3 = document.getElementById("placa3");
var placa4 = document.getElementById("placa4");

continuar.addEventListener("click", (e) => {
	var pasar = 1;
	if (nombre.value.length < 1) {
		divNombre.lastElementChild.textContent = "Nombre NO válido";
		pasar = 0;
	} else {
		divNombre.lastElementChild.textContent = "";
	}
	if (primerApellido.value.length < 1) {
		divApellido1.lastElementChild.textContent = "Apellido NO válido";
		pasar = 0;
	} else {
		divApellido1.lastElementChild.textContent = "";
	}
	if (segundoApellido.value.length < 1) {
		divApellido2.lastElementChild.textContent = "Apellido NO válido";
		pasar = 0;
	} else {
		divApellido2.lastElementChild.textContent = "";
	}
	if (identificacion.value.length < 1) {
		divIdentificacion.lastElementChild.textContent = "Identificación NO válido";
		pasar = 0;
	} else {
		divIdentificacion.lastElementChild.textContent = "";
	}
	var valor = vehiculo.value;
	divPlaca4.lastElementChild.hidden = true;
	divPlaca3.lastElementChild.hidden = true;
	divPlaca2.lastElementChild.hidden = true;
	divPlaca1.lastElementChild.hidden = true;
	if (valor != "0") {
		switch (valor) {
			case "4":
				if (placa4.value == "") {
					divPlaca4.lastElementChild.hidden = false;
					pasar = 0;
				} 
			case "3":
				if (placa3.value == "") {
					divPlaca3.lastElementChild.hidden = false;
					pasar = 0;
				}
			case "2":
				if (placa2.value == "") {
					divPlaca2.lastElementChild.hidden = false;
					pasar = 0;
				}
			case "1":
				if (placa1.value == "") {
					divPlaca1.lastElementChild.hidden = false;
					pasar = 0;
				}
				break;
		}
	}
	if (pasar > 0) {
		location.href = '../Home/FinalizarReserva';
	}
});

vehiculo.addEventListener("change", (e) => {
	var valor = vehiculo.value;
	var label1 = document.getElementById("label-placa1");
	var label2 = document.getElementById("label-placa2");
	var label3 = document.getElementById("label-placa3");
	var label4 = document.getElementById("label-placa4");
	divPlaca4.lastElementChild.hidden = true;
	divPlaca3.lastElementChild.hidden = true;
	divPlaca2.lastElementChild.hidden = true;
	divPlaca1.lastElementChild.hidden = true;
	switch (valor) {
		case "0":
			placa1.hidden = true;
			label1.hidden = true;
		case "1":
			placa2.hidden = true;
			label2.hidden = true;
		case "2":
			placa3.hidden = true;
			label3.hidden = true;
		case "3":
			placa4.hidden = true;
			label4.hidden = true;
			break;
	}
	switch (valor) {
		case "4":
			placa4.hidden = false;
			label4.hidden = false;
		case "3":
			placa3.hidden = false;
			label3.hidden = false;
		case "2":
			placa2.hidden = false;
			label2.hidden = false;
		case "1":
			placa1.hidden = false;
			label1.hidden = false;
			break;
	}
});