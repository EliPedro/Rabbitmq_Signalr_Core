using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ_SignalR_Service.DTO;
using System.Diagnostics;
using System.Text;

namespace RabbitMQ_SignalR_Service.Service
{
    public class ServiceQueue
    {
        public static void SendMessageToQueue(Mensagem mensagem, string routingKey)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Chat",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.ExchangeDeclare("exchangeChat", ExchangeType.Topic);
                channel.QueueBind("Chat", "exchangeChat", routingKey);

                var pesquisa = JsonConvert.SerializeObject(mensagem);

                var body = Encoding.UTF8.GetBytes(pesquisa);

                channel.BasicPublish(exchange: "exchangeChat",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
                Debug.WriteLine("[x] Enviando {0}", mensagem.Texto);
            }
        }
    }
}
