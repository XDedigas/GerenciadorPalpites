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
}

function tamanhoPaginaChanged() {
    $('#hiddenTamanhoPagina').val($('#ddl_tam_pag').val());
    document.forms['FormTamPag'].submit();
}

function myCustomFunc(data) {
    console.log(data);
    return data;
}