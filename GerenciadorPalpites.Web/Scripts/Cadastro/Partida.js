function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#ddl_campeonato').val(dados.Campeonato);
    $('#ddl_timeCasa').val(dados.TimeCasa);
    $('#ddl_timeFora').val(dados.TimeFora);
    $('#txt_data').val(dados.Data);
    $('#txt_placarTimeCasa').val(dados.PlacarTimeCasa);
    $('#txt_placarTimeFora').val(dados.PlacarTimeFora);
}

function set_focus_form() {
    var alterando = (parseInt($('#id_cadastro').val()) > 0);
    $('#txt_quant_estoque').attr('readonly', alterando);

    $('#txt_codigo').focus();
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Campeonato: 0,
        TimeCasa: 0,
        TimeFora: 0,
        Data: '',
        PlacarTimeCasa: 0,
        PlacarTimeFora: 0,
    };
}

function get_dados_form() {
    var form = new FormData();
    form.append('Id', $('#id_cadastro').val());
    form.append('Campeonato', $('#ddl_campeonato').val());
    form.append('TimeCasa', $('#ddl_timeCasa').val());
    form.append('TimeFora', $('#ddl_timeFora').val());
    form.append('Data', $('#txt_data').val());
    form.append('PlacarTimeCasa', $('#txt_placarTimeCasa').val());
    form.append('PlacarTimeFora', $('#txt_placarTimeFora').val());
    return form;
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Id).end()
        .eq(1).html(param.Campeonato).end()
        .eq(2).html(param.TimeCasa).end()
        .eq(3).html(param.TimeFora).end()
        .eq(4).html(param.Data).end()
        .eq(5).html(param.PlacarTimeFora).end()
        .eq(6).html(param.PlacarTimeCasa).end();
}

function salvar_customizado(url, param, salvar_ok, salvar_erro) {
    $.ajax({
        type: 'POST',
        processData: false,
        contentType: false,
        data: param,
        url: url,
        dataType: 'json',
        success: function (response) {
            salvar_ok(response, get_param());
        },
        error: function () {
            salvar_erro();
        }
    });
}

function get_param() {
    return {
        Id: $('#id_cadastro').val(),
        Campeonato: $('#ddl_campeonato').val(),
        TimeCasa: $('#ddl_timeCasa').val(),
        TimeFora: $('#ddl_timeFora').val(),
        Data: $('#txt_data').val(),
        PlacarTimeCasa: $('#txt_palcarTimeCasa').val(),
        PlacarTimeFora: $('#txt_palcarTimeFora').val()
    };
}