

using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using SOP.Messages.Messages;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SOP.WebClient
{
    class Program
    {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private static HubConnection _hub;
        private const string SUBSCRIBER_ID = "SOP.WebClient";
        private static IBus _bus;

        static async Task Main(string[] args)
        {
            _bus = RabbitHutch.CreateBus(config.GetConnectionString("SOPRabbitMQ"));
            Console.WriteLine("Connected to bus!");

            _hub = new HubConnectionBuilder().WithUrl(config.GetConnectionString("SignalR_Hub_Url")).Build();
            await _hub.StartAsync();
            Console.WriteLine("Hub started");

            var ownerAgeMessages = _bus.PubSub.SubscribeAsync<NewOwnerAgeMessage>(SUBSCRIBER_ID, HandleNewOwnerMessage);
            Task.WaitAll(ownerAgeMessages);

            Console.ReadKey(true);
        }

        private static async Task HandleNewOwnerMessage(NewOwnerAgeMessage message)
        {
            var csvRow = $"{message.Years} {message.Months} {message.Days} : {message.Name}, {message.Surname}, " +
                     $"{message.VehicleRegistration}, {message.Birthday}, {message.Email}, {message.ListedAtUtc}";

            Console.WriteLine(csvRow);

            var json = JsonSerializer.Serialize(message, JsonSettings());

            await _hub.SendAsync("NotifyWebUsers", "SOP.SignalRClient", json);
        }

        
        private static IConfigurationRoot ReadConfiguration()
        {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        static JsonSerializerOptions JsonSettings() =>
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
    }
}
