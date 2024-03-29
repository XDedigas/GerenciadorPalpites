﻿function setFocusForm() {
    $('#ddl_time').focus();
}

function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);

    var inclusao = (dados.Id == 0);
    if (inclusao) {
        $('#ddl_estado').empty();
        $('#ddl_estado').prop('disabled', true);

        $('#ddl_cidade').empty();
        $('#ddl_cidade').prop('disabled', true);
    }
    else {
        $('#ddl_pais').val(dados.IdPais);
        mudar_pais(dados.IdEstado, dados.IdCidade);
    }
}

function mudar_pais(idEstado, idCidade) {
    var ddl_pais = $('#ddl_pais'),
        idPais = parseInt(ddl_pais.val()),
        ddl_estado = $('#ddl_estado'),
        ddl_cidade = $('#ddl_cidade');

    if (idPais > 0) {
        var url = url_listar_estados,
            param = { idPais: idPais };

        ddl_estado.empty();
        ddl_estado.prop('disabled', true);

        ddl_cidade.empty();
        ddl_cidade.prop('disabled', true);

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response && response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    ddl_estado.append('<option value=' + response[i].Id + '>' + response[i].Nome + '</option>');
                }
                ddl_estado.prop('disabled', false);
            }
            sel_estado(idEstado);
            mudar_estado(idCidade);
        });
    }
}

function mudar_estado(idCidade) {
    var ddl_estado = $('#ddl_estado'),
        idEstado = parseInt(ddl_estado.val()),
        ddl_cidade = $('#ddl_cidade');

    if (idEstado > 0) {
        var url = url_listar_cidades,
            param = { idEstado: idEstado };

        ddl_cidade.empty();
        ddl_cidade.prop('disabled', true);

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response && response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    ddl_cidade.append('<option value=' + response[i].Id + '>' + response[i].Nome + '</option>');
                }
                ddl_cidade.prop('disabled', false);
            }
            sel_cidade(idCidade);
        });
    }
}

function sel_estado(idEstado) {
    $('#ddl_estado').val(idEstado);
    $('#ddl_estado').prop('disabled', $('#ddl_estado option').length == 0);
}

function sel_cidade(idCidade) {
    $('#ddl_cidade').val(idCidade);
    $('#ddl_cidade').prop('disabled', $('#ddl_cidade option').length == 0);
}

function set_focus_form() {
    $('#txt_nome').focus();
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        IdPais: 0,
        IdEstado: 0,
        IdCidade: 0
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        IdPais: $('#ddl_pais').val(),
        IdEstado: $('#ddl_estado').val(),
        IdCidade: $('#ddl_cidade').val()
    };
}

function preencher_linha_grid(param, linha) {
    linha.eq(0).html(param.Nome).end();
}

$(document)
    .on('change', '#ddl_pais', function () {
        mudar_pais();
    })
    .on('change', '#ddl_estado', function () {
        mudar_estado();
    }).on('click', '#btn_comparar', function () {
        var btn = $(this),
            id = btn.closest('tr').attr('data-id'),
            modalCadastro = $('#modal_cadastro_comparar');
        document.getElementById('infoTime1').value = id;

        $('#msg_mensagem_aviso_comparar').empty();
        $('#msg_aviso_comparar').hide();
        $('#msg_mensagem_aviso_comparar').hide();
        $('#msg_erro_comparar').hide();

        bootbox.dialog({
            title: 'Comparador de Times',
            message: modalCadastro,
            className: 'dialogo',
        }).on('shown.bs.modal', function () {
            modalCadastro.show(0, function () {
                setFocusForm();
            });
        }).on('hidden.bs.modal', function () {
            modalCadastro.hide().appendTo('body');
        });;
    });