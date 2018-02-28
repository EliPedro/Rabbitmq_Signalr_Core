using RabbitMQ_SignalR_Service.DTO;
using RabbitMQ_SignalR_Service.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CoreMvc.Hubs
{
    public class ChatHub : Hub
    {
        public void SendMessage(string message, string connection)
        {
            var msg = new Mensagem
            {
                Conexao = connection,
                Texto = message
            };

            ServiceQueue.SendMessageToQueue(msg, "chat");
        }

        public async Task PushNotify(Mensagem message)
        {
            await Clients.User(message.Conexao).InvokeAsync("newMessage", message.Texto);
        }
    }
}
