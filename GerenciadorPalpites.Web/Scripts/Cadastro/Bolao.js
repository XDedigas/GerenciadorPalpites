function setFocusForm() {
    $('#txt_nome').focus();
}

function getDadosForm() {
    return {
        Id: ($('#id_cadastro').val()) ? $('#id_cadastro').val() : 0,
        Nome: $('#txt_nome').val(),
        IdCampeonato: $('#ddl_campeonato').val()
    };
}