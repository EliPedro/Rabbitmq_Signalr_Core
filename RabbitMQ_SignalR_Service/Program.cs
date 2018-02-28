using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ_SignalR_Service.DTO;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_SignalR_Service
{
    class Program
    {
        static HubConnection _connection;
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                Connect().Wait();

                channel.QueueDeclare(queue: "Chat",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;


                channel.BasicConsume(queue: "Chat",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Aguardando mensagens para processamento");
                Console.WriteLine("Pressione uma tecla para encerrar...");
                Console.ReadKey();
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine(Environment.NewLine + "[Nova mensagem recebida] " + message);
            
            PushNotify(message);
        }

        private static void PushNotify(string msg)
        {
            var message = JsonConvert.DeserializeObject<Mensagem>(msg);
            
            _connection.InvokeAsync("PushNotify", message);
        }

        private static async Task Connect()
        {
            try
            {
                _connection = new HubConnectionBuilder()
                 .WithUrl("http://localhost:63516/chat")
                 .WithConsoleLogger()
                 .WithMessagePackProtocol()
                 .WithTransport(TransportType.WebSockets)
                 .Build();

                await _connection.StartAsync();
                _connection.On<string>("newMessage", (texto) => { });

            }
            catch (Exception ex)
            {
               Debug.WriteLine(ex.ToString());
            }           
        }
    }
}

// Install-Package Microsoft.AspNetCore.SignalR.Client -Version 1.0.0-alpha2-final	
// Install-Package Microsoft.AspNetCore.SignalR -Version 1.0.0-alpha2-final	

// rabbitmq-plugins enable rabbitmq_management
// localhost:15672
