// -------- Validación de especialidades--------
function validarEspecialidades(sender, args) {
    const cont = document.querySelector('[data-rol="especialidades"]');
    if (!cont) { args.IsValid = false; return; }

    const checks = Array.from(cont.querySelectorAll('input[type=checkbox]'));
    const sel = checks.filter(c => c.checked).length;

    if (sel === 0) {
        sender.errormessage = 'Debe seleccionar al menos una especialidad.';
        args.IsValid = false;
        return;
    }
    if (sel > 2) {
        sender.errormessage = 'Solo puede seleccionar hasta 2 especialidades.';
        args.IsValid = false;
        return;
    }
    args.IsValid = true;
}

// Bloqueo visual al llegar a 2
window.addEventListener('load', () => {
    const cont = document.querySelector('[data-rol="especialidades"]');
    if (!cont) return;

    const checks = cont.querySelectorAll('input[type=checkbox]');
    let aviso = cont.nextElementSibling;
    if (!aviso || !aviso.classList.contains('text-danger')) {
        aviso = document.createElement('small');
        aviso.className = 'text-danger d-block mt-1';
        cont.insertAdjacentElement('afterend', aviso);
    }

    const max = 2;
    const sync = () => {
        const seleccionados = Array.from(checks).filter(c => c.checked);
        if (seleccionados.length >= max) {
            aviso.textContent = `Solo podés elegir ${max} especialidades.`;
            checks.forEach(c => { if (!c.checked) c.disabled = true; });
        } else {
            aviso.textContent = '';
            checks.forEach(c => c.disabled = false);
        }
    };
    checks.forEach(chk => chk.addEventListener('change', sync));
    sync();
});

// -------- UI: mostrar/ocultar paneles --------
function mostrarFormularioMedico() {
    const formPanel = document.querySelector('[data-rol="panel-form"]');
    const listPanel = document.querySelector('[data-rol="panel-lista"]');
    if (formPanel) formPanel.style.display = 'block';
    if (listPanel) listPanel.style.display = 'none';
    if (formPanel) window.scrollTo({ top: formPanel.offsetTop - 80, behavior: 'smooth' });
}

function ocultarFormularioMedico() {
    const formPanel = document.querySelector('[data-rol="panel-form"]');
    const listPanel = document.querySelector('[data-rol="panel-lista"]');
    if (formPanel) formPanel.style.display = 'none';
    if (listPanel) listPanel.style.display = 'block';
}

// Limpiar formulario y forzar modo ALTA
function limpiarFormularioMedico() {
    const setVal = (sel, val = '') => { const el = document.querySelector(sel); if (el) el.value = val; };

    setVal('[data-rol="inp-nombre"]');
    setVal('[data-rol="inp-apellido"]');
    setVal('[data-rol="inp-telefono"]');
    setVal('[data-rol="inp-email"]');
    setVal('[data-rol="inp-matricula"]');
    setVal('[data-rol="inp-pass"]');

    const hf = document.getElementById('hfMedicoId');
    if (hf) hf.value = '';

    const ddl = document.querySelector('[data-rol="ddl-turno"]');
    if (ddl) ddl.selectedIndex = 0;

    const cont = document.querySelector('[data-rol="especialidades"]');
    if (cont) cont.querySelectorAll('input[type=checkbox]').forEach(c => { c.checked = false; c.disabled = false; });

    const vt = document.querySelector('[data-rol="form-title"]');
    if (vt) vt.textContent = 'Nuevo médico';
}

// Botones (alta / cancelar)
window.addEventListener('DOMContentLoaded', () => {
    const btnAlta = document.querySelector('[data-rol="btn-alta"]');
    const btnCancelar = document.querySelectorAll('[data-rol="btn-cancelar"]');

    if (btnAlta) btnAlta.addEventListener('click', () => { limpiarFormularioMedico(); mostrarFormularioMedico(); });
    btnCancelar.forEach(b => b.addEventListener('click', () => ocultarFormularioMedico()));
});


window.abrirModalReprog = function () {
    var m = new bootstrap.Modal(document.getElementById('mdlReprog'));
    m.show();
};


// ======= Toast universal y cola segura =======
(function () {
    
    function defineToastOnce() {
        if (window.mostrarToast) return true;

        if (!(window.bootstrap && bootstrap.Toast)) {
           
            return false;
        }

        window.mostrarToast = function (mensaje, tipo = "success", delayMs = 2500) {
            var cont = document.querySelector('.toast-container');
            if (!cont) {
                cont = document.createElement('div');
                cont.className = 'toast-container position-fixed bottom-0 end-0 p-3';
                cont.style.zIndex = 99999;
                document.body.appendChild(cont);
            }

            var toast = document.createElement('div');
            toast.className = "toast align-items-center text-bg-" + tipo + " border-0";
            toast.setAttribute("role", "status");
            toast.innerHTML =
                '<div class="d-flex">' +
                '<div class="toast-body">' + (mensaje || '') + '</div>' +
                '<button type="button" class="btn-close btn-close-white me-2 m-auto" ' +
                'data-bs-dismiss="toast" aria-label="Cerrar"></button>' +
                '</div>';

            cont.appendChild(toast);
            var inst = bootstrap.Toast.getOrCreateInstance(toast, { delay: delayMs });
            inst.show();
            toast.addEventListener('hidden.bs.toast', function () { toast.remove(); });
        };

        return true;
    }

    
    function drainQueue() {
        if (!defineToastOnce()) {
           
            setTimeout(drainQueue, 50);
            return;
        }

        if (!Array.isArray(window.__queueToast)) return;

        var q = window.__queueToast.splice(0); 
        q.forEach(function (x) {
            try {
                window.mostrarToast(x.m, x.t, x.d);
                if (x.redirect) {
                    setTimeout(function () { window.location = x.redirect; }, x.d || 1500);
                }
            } catch (e) {  }
        });
    }

  
    if (window.Sys && Sys.Application) {
        Sys.Application.add_load(drainQueue);
    }
    window.addEventListener('load', drainQueue);
})();
