using RabbitMQ_SignalR_Service.DTO;
using RabbitMQ_SignalR_Service.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using CoreMvc.Models;
using Microsoft.AspNetCore.Identity;

namespace CoreMvc.Hubs
{

    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public void SendMessage(string message)
        {
            var msg = new Mensagem
            {
                Conexao = Context.ConnectionId,
                Texto = message
            };

            ServiceQueue.SendMessageToQueue(msg, "chat");
        }

        public async Task PushNotify(Mensagem message)
        {
            var user = await _userManager.FindByIdAsync("ee162d67-329c-4a07-9544-3b9fee70c2eb");
            await Clients.User(user.UserName).InvokeAsync("newMessage", message.Texto);
        }
    }
}
