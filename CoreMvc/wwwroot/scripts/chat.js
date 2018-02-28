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
        var data = $("#mensagem-chat").val();
        connection.invoke('SendMessage', data);
        $("#mensagem-chat").val('');
    });    
});





