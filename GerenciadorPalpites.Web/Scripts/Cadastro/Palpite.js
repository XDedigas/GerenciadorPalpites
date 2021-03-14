function clearErrorMessages() {
    var element = $('#msg_erro_salvar_palpites');
    if (element) {
        element.html('');
        element.addClass('invisivel');
    }
}

function showErrorMessage(message) {
    var element = $('#msg_erro_salvar_palpites');
    if (element) {
        element.removeClass('invisivel');
        element.html(message);
    }
}

function showMultipleWarningMessages(messages) {
    var element = $('#msg_erro_salvar_palpites');
    if (element) {
        var finalMessage = '';
        for (var i = 0; i < messages.length; i++) {
            var message = messages[i].mensagem;
            finalMessage += message + '<br />';
        }

        element.removeClass('invisivel');
        element.html(finalMessage);
    }
}

function getPalpites() {
    var palpites = [];
    var erros = [];

    var cards = document.querySelectorAll('.palpite');
    for (var i = 0; i < cards.length; i++) {
        var current = cards[i];
        var properties = JSON.parse(current.getAttribute('data-item'));
        var componentePalpiteTimeCasa = document.querySelector(`.palpite[data-idpartida="${properties.idPartida}"] #palpiteTimeCasa`);
        var palpiteTimeCasa = componentePalpiteTimeCasa.value;
        var componentePalpiteTimeFora = document.querySelector(`.palpite[data-idpartida="${properties.idPartida}"] #palpiteTimeFora`);
        var palpiteTimeFora = componentePalpiteTimeFora.value;

        if ((!palpiteTimeCasa || isNaN(palpiteTimeCasa) || parseInt(palpiteTimeCasa) < 0) ||
            (!palpiteTimeFora || isNaN(palpiteTimeFora) || parseInt(palpiteTimeFora) < 0)) {
            erros.push({ mensagem: `O palpite da partida entre ${properties.timeCasa} e ${properties.timeFora} não é válido!` });
        } else {
            palpites.push({ "Id": properties.id, "IdPartida": properties.idPartida, "IdTimeCasa": properties.idTimeCasa, "IdTimeFora": properties.idTimeFora, "PalpiteTimeCasa": parseInt(palpiteTimeCasa), "PalpiteTimeFora": parseInt(palpiteTimeFora), "IdBolao": idBolao });
        }
    }

    return { palpites, erros };
}

$(document).on('click', '#btn_classificacao', function () {
    window.location.href = urlClassificacao;
}).on('click', '#btn_salvar_palpites', function () {
    clearErrorMessages();
    var result = getPalpites();
    if (result.erros.length > 0) {
        showMultipleWarningMessages(result.erros);
    } else {
        $.ajax({
            type: 'POST',
            url: urlSalvarPalpites,
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(result.palpites),
            success: function (data) {
                if (data.Resultado === "OK") {
                    window.location.reload(true);
                    swal("Sucesso!", "Palpite registrado!", "success");
                } else if (data.Resultado === 'AVISO') {
                    showErrorMessage(data.Mensagens);
                } else {
                    showErrorMessage('Não foi possível salvar os palpites. Tente novamente mais tarde.');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                showErrorMessage('Não foi possível salvar os palpites. Tente novamente mais tarde.');
            }
        });
    }
});