function validarEspecialidades(sender, args) {
    var inputs = document.querySelectorAll('#<%= chkEspecialidades.ClientID %> input[type=checkbox]');
    args.IsValid = Array.from(inputs).some(i => i.checked);
}
document.addEventListener("DOMContentLoaded", function () {
    const lista = document.querySelector('[data-rol="especialidades"]');
    if (!lista) return; // si no está en esta página, no hace nada

    const checkboxes = lista.getElementsByTagName('input');
    const maxSeleccion = 2;

    // Mensaje debajo del listado
    const aviso = document.createElement("span");
    aviso.className = "text-danger small d-block mt-1";
    lista.after(aviso);

    const actualizarEstado = () => {
        const seleccionados = Array.from(checkboxes).filter(c => c.checked);
        const cantidad = seleccionados.length;

        if (cantidad >= maxSeleccion) {
            aviso.textContent = `Solo podés seleccionar hasta ${maxSeleccion} especialidades.`;
            checkboxes.forEach(chk => {
                if (!chk.checked) {
                    chk.disabled = true;
                    chk.closest("label, span, td").classList.add("opacity-50"); // efecto visual
                }
            });
        } else {
            aviso.textContent = "";
            checkboxes.forEach(chk => {
                chk.disabled = false;
                chk.closest("label, span, td").classList.remove("opacity-50");
            });
        }
    };

    checkboxes.forEach(chk => {
        chk.addEventListener("change", actualizarEstado);
    });
});
