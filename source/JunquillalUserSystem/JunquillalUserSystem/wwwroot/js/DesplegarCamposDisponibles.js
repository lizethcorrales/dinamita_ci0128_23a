var fechaEntrada = document.getElementById("fecha-entrada");
var casetilla = document.getElementById("casetilla");
var online = document.getElementById("online");

buscar.addEventListener("click", (e) => {
    console.log("Entra");
    console.log(fechaEntrada.value);
    if (fechaEntrada.value.length != 0) {
        //casetilla.setAttribute("value", "30");
        casetilla.textContent = "30";
    }
});