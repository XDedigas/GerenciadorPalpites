$(document).ready(function () {
    var grid = $('#grid_cadastro > tbody');
    for (var i = 0; i < linhas.length; i++) {
        grid.append(criar_linha_grid(linhas[i]));
    }

    marcar_ordenacao_campo();
});

function criar_linha_grid(dados, confirmar) {
    var template = null;
    if (confirmar != null) {
        template = $('#template-grid-confirmar').html();
    } else {
        template = $('#template-grid-alterar').html();
    }

    return Mustache.render(template, dados);
}

function marcar_ordenacao_campo() {
    const url = window.location.href;

    //Verifica se a URL tem query string
    if (url.indexOf('?') > 0) {
        const urlQuery = url.substring(url.indexOf('?') + 1);
        const queryParams = urlQuery.split('&');
        const orderParam = queryParams.find(parameter => parameter.startsWith('ordenacao'));

        if (orderParam) {
            const [name, order] = orderParam.substring(orderParam.indexOf('=') + 1).split('%20');
            if (name) {
                const gridColumn = $(`a[data-column-sort="${name}"]`);

                if (gridColumn) {
                    if (order) {
                        gridColumn.append('&nbsp;<i class="glyphicon glyphicon-arrow-up" style="color: #000000"></i>');
                    } else {
                        gridColumn.append('&nbsp;<i class="glyphicon glyphicon-arrow-down" style="color: #000000"></i>');
                    }
                }
            } else {
                const firstColumn = $('#grid_cadastro thead tr th:nth-child(1) a');
                firstColumn.append('&nbsp;<i class="glyphicon glyphicon-arrow-down" style="color: #000000"></i>');
            }
        } else {
            const firstColumn = $('#grid_cadastro thead tr th:nth-child(1) a');
            firstColumn.append('&nbsp;<i class="glyphicon glyphicon-arrow-down" style="color: #000000"></i>');
        }
    } else {
        const firstColumn = $('#grid_cadastro thead tr th:nth-child(1) a');
        firstColumn.append('&nbsp;<i class="glyphicon glyphicon-arrow-down" style="color: #000000"></i>');
    }
}

function tamanhoPaginaChanged() {
    $('#hiddenTamanhoPagina').val($('#ddl_tam_pag').val());
    document.forms['FormTamPag'].submit();
}

function addAntiForgeryToken(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}

function formatWarningMessage(mensagens) {
    var template =
        '<ul>' +
        '{{ #. }}' +
        '<li>{{ . }}</li>' +
        '{{ /. }}' +
        '</ul>';

    return Mustache.render(template, mensagens);
}

function successfulResponse(response) {
    if (response.Resultado === 'OK') {
        window.location.href = response.Url;
    } else if (response.Resultado === 'ERRO') {
        $('#msg_aviso').hide();
        $('#msg_mensagem_aviso').hide();
        $('#msg_erro').show();
    } else if (response.Resultado === 'AVISO') {
        $('#msg_mensagem_aviso').html(formatWarningMessage(response.Mensagens));
        $('#msg_aviso').show();
        $('#msg_mensagem_aviso').show();
        $('#msg_erro').hide();
    }
}

function failedResponse() {
    swal('Aviso', 'Não foi possível salvar. Tente novamente em instantes.', 'warning');
}

$(document).on('click', '#btn_incluir', function () {
    var modalCadastro = $('#modal_cadastro');

    $('#msg_mensagem_aviso').empty();
    $('#msg_aviso').hide();
    $('#msg_mensagem_aviso').hide();
    $('#msg_erro').hide();

    bootbox.dialog({
        title: 'Cadastro de ' + tituloPagina,
        message: modalCadastro,
        className: 'dialogo',
    }).on('shown.bs.modal', function () {
        modalCadastro.show(0, function () {
            setFocusForm();
        });
    }).on('hidden.bs.modal', function () {
        modalCadastro.hide().appendTo('body');
    });;
}).on('click', '#btn_confirmar', function () {
    var url = urlSalvar;
    var data = getDadosForm();

    $.post(url, addAntiForgeryToken(data), function (response) {
        successfulResponse(response);
    }).fail(function () {
        failedResponse();
    });
});