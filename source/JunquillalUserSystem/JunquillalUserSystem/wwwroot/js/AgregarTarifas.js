
var nacionalidad = document.getElementById("Nacionalidad");
nacionalidad.addEventListener("change", (e) => {
    var valor = nacionalidad.value;
    var precio = document.getElementById('Precio');
        switch (valor) {
            case "Nacional":
                precio.setAttribute('placeholder', 'Colones');
                break;
            case "Extranjero":
                precio.setAttribute('placeholder', 'Dólares');
                break;
        default:
                precio.setAttribute('placeholder', '$ ó ₡');
                break;
    }
});