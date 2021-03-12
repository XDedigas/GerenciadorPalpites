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

function participarComSenha() {
    debugger;
    var senha = document.getElementById('txt_senha_participar').value,
        url = url_participar,
        param = { 'idBolao': idBolao, 'nomeUsuario': nomeUsuario, 'senha': senha };

    $.post(url, add_anti_forgery_token(param), function (response) {
        if (response) {
            if (response.Resultado == "ERRO") {
                swal('Aviso', 'Não foi possível participar do bolão. Tente novamente em instantes.', 'warning');
            }
            else {
                var tr = document.querySelector(`tr[data-id="${idBolao}"]`);
                tr.remove();
                var quant = $('#grid_cadastro > tbody > tr').length;
                if (quant == 0) {
                    $('#grid_cadastro').addClass('invisivel');
                    $('#mensagem_grid').removeClass('invisivel');
                }
            }
        }
    }).fail(function () {
        swal('Aviso', 'Não foi possível participar do bolão. Tente novamente em instantes.', 'warning');
    });
}

$(document).on('click', '.btn-participar', function () {
    var btn = $(this),
        tr = btn.closest('tr'),
        id = tr.attr('data-id'),
        modal_cadastro_participar = $('#modal_cadastro_participar');

    idBolao = id;

    $('#msg_mensagem_aviso_participar').empty();
    $('#msg_aviso_participar').hide();
    $('#msg_mensagem_participar').hide();
    $('#msg_erro_participar').hide();

    $.ajax({
        type: 'POST',
        processData: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'idBolao': id }),
        url: url_VerificaPublico,
        success: function (response) {
            var isPublic = response.IsPublic;
            if (isPublic) {
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
            }
            else {
                bootbox.dialog({
                    title: 'Participar do Bolão',
                    message: modal_cadastro_participar,
                    className: 'dialogo',
                }).on('shown.bs.modal', function () {
                    modal_cadastro_participar.show(0, function () {
                        setFocusForm();
                    });
                }).on('hidden.bs.modal', function () {
                    modal_cadastro_participar.hide().appendTo('body');
                });
            }
        },
        error: function () {
            swal('Aviso', 'Não foi possível realizar a comparação. Tente novamente em instantes.', 'warning');
        }
    });
});