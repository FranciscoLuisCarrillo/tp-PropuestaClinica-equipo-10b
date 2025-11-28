function validarEspecialidades(sender, args) {
    const lista = document.querySelector('[data-rol="especialidades"]');
    if (!lista) { args.IsValid = false; return; }
    args.IsValid = [...lista.querySelectorAll('input[type=checkbox]')]
        .some(c => c.checked);
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

function mostrarFormularioMedico() {
    const formPanel = document.getElementById('pnlNuevoMedico');
    const listPanel = document.getElementById('pnlListadoMedicos');

    if (formPanel) formPanel.style.display = 'block';
    if (listPanel) listPanel.style.display = 'none';

    if (formPanel) {
        window.scrollTo({ top: formPanel.offsetTop - 80, behavior: 'smooth' });
    }
}

function ocultarFormularioMedico() {
    const formPanel = document.getElementById('pnlNuevoMedico');
    const listPanel = document.getElementById('pnlListadoMedicos');

    if (formPanel) formPanel.style.display = 'none';
    if (listPanel) listPanel.style.display = 'block';
}

window.abrirModalPassEmail = function (email) {
    var hidEmail = document.getElementById('hfEmailDestino');
    var lbl = document.getElementById('lblEmailModal');
    var txt = document.getElementById('txtNuevaPass');
    var modalEl = document.getElementById('modalPassword');

    if (hidEmail) hidEmail.value = email || '';
    if (lbl) lbl.textContent = email || '';
    if (txt) txt.value = '';

    if (modalEl && typeof bootstrap !== 'undefined') {
        var m = new bootstrap.Modal(modalEl);
        m.show();
    } else {
        console.error('Modal/Bootstrap no disponible.');
    }
};