function setFocusForm() {
    $('#txt_nome').focus();
}

function getDadosForm() {
    return {
        Id: ($('#id_cadastro').val()) ? $('#id_cadastro').val() : 0,
        Nome: $('#txt_nome').val(),
        IdCampeonato: $('#ddl_campeonato').val(),
        Publico: $('#chk_privado').is(':checked'),
        Senha: $('#txt_senha').val(),
        AlterarPontuacao: $('#chk_alterarPontuacao').is(':checked'),
        PlacarExato: $('#txt_placarExato').val(),
        AcertouVencedor: $('#txt_acertarVencedor').val(),
        GolsFeitos: $('#txt_golsTimes').val(),
    };
}

function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}

$(document).on('click', '.btn-participar', function () {
    var btn = $(this),
        tr = btn.closest('tr'),
        id = tr.attr('data-id'),
        url = url_participar,
        param = { 'idBolao': id, 'nomeUsuario': nomeUsuario };

    bootbox.confirm({
        message: "Realmente deseja participar do bolão?",
        buttons: {
            confirm: {
                label: 'Sim',
                className: 'btn-success'
            },
            cancel: {
                label: 'Não',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.post(url, add_anti_forgery_token(param), function (response) {
                    if (response) {
                        tr.remove();
                        var quant = $('#grid_cadastro > tbody > tr').length;
                        if (quant == 0) {
                            $('#grid_cadastro').addClass('invisivel');
                            $('#mensagem_grid').removeClass('invisivel');
                        }
                        
                    }
                }).fail(function () {
                    swal('Aviso', 'Não foi possível participar do bolão. Tente novamente em instantes.', 'warning');
                });
            }
        }
    });
})