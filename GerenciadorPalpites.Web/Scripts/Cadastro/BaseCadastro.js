var myBarChart;

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
function attGrafico() {
    var idtime1 = document.getElementById('infoTime1').value;
    var idtime2 = $('#ddl_time').val();

    $.ajax({
        type: 'POST',
        processData: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'idTime1': idtime1, 'idTime2': idtime2 }),
        url: document.getElementById('url_comparar').value,
        success: function (response) {
            if (response.VitoriasTimeA === 0 && response.VitoriasTimeB === 0 && response.Empates === 0) {
                swal('Aviso', 'Não existem estatísticas entre esses times.', 'warning');
            }
            else {
                var ctx = document.getElementsByClassName("pie-chart");

                if (myBarChart) {
                    myBarChart.destroy();
                }

                myBarChart = new Chart(ctx, {
                    type: 'pie',
                    data: {
                        labels: [
                            response.NomeTimeA,
                            response.NomeTimeB,
                            "Empates"
                        ],
                        datasets: [{
                            label: 'Estatisticas',
                            data: [response.VitoriasTimeA, response.VitoriasTimeB, response.Empates],
                            backgroundColor: [
                                'rgb(255, 99, 132)',
                                'rgb(54, 162, 235)',
                                'rgb(255, 205, 86)'
                            ],
                            hoverOffset: 4
                        }]
                    }
                });
            }
        },
        error: function () {
            swal('Aviso', 'Não foi possível realizar a comparação. Tente novamente em instantes.', 'warning');
        }
    });
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
}).on('click', '#btn_entrar', function () {
    var btn = $(this),
        tr = btn.closest('tr'),
        idBolao = tr.attr('data-id');
    window.location.href = `${urlPalpite}?idBolao=${idBolao}`;
}).on('click', '#btn_palpites', function () {
    window.location.href = urlPalpites;
});