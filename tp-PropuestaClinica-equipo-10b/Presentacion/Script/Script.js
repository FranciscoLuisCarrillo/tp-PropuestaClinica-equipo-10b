function validarEspecialidades(sender, args) {
    const lista = document.querySelector('[data-rol="especialidades"]');
    if (!lista) { args.IsValid = false; return; }
    args.IsValid = [...lista.querySelectorAll('input[type=checkbox]')].some(c => c.checked);
}
window.addEventListener('load', function () {
    const lista = document.querySelector('[data-rol="especialidades"]');
    if (!lista) return;

    const checkboxes = lista.querySelectorAll('input[type=checkbox]');
    const aviso = document.createElement('small');
    aviso.className = 'text-danger d-block mt-1';
    lista.insertAdjacentElement('afterend', aviso);

    checkboxes.forEach(chk => chk.addEventListener('change', () => {
        const seleccionados = [...checkboxes].filter(c => c.checked);
        const max = 2;

        if (seleccionados.length >= max) {
            aviso.textContent = `Solo podés elegir ${max} especialidades.`;
            checkboxes.forEach(c => !c.checked && (c.disabled = true));
        } else {
            aviso.textContent = '';
            checkboxes.forEach(c => c.disabled = false);
        }
    }));
});