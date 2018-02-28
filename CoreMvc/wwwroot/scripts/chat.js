$(document).ready(function () {
    
    var transportType = signalR.TransportType.WebSockets;
    var logger = new signalR.ConsoleLogger(signalR.LogLevel.Information);
    var chatHub = new signalR.HttpConnection(`http://${document.location.host}/chat`,
                      { transport: transportType, logger: logger });
    var connection = new signalR.HubConnection(chatHub, logger);

    connection.start().catch(err => {
        console.log('connection error');
    });

    connection.on('newMessage', texto => {

        $("#nova-mensagem").append("<div>" + texto + "</div>");
    });

    $("#btn-enviar").click(function () {
        var mensagem = $("#mensagem-chat").val();
        
        $("#nova-mensagem").append("<div><h3 style='color:#0000FF'>" + mensagem + "</h3></div>");

        var user = $("#ClienteId").select().val();
        var usrName = $("#user").val();

        connection.invoke('SendMessage', (usrName + " : " + mensagem), user);
        $("#mensagem-chat").val('');
    });    
});





