

function CarregaPaginaCentral(url) {
    $.ajax({
        url: url, 
        type: "POST",
        success: function (data) {
            $("#content-container").html(data); 
        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }
    });
}
