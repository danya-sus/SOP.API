
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using SOP.Messages.Messages;

namespace SOP_CW.AgeClient
{
    class Program
    {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private const string SUBSCRIBER_ID = "SOP_CW.AgeClient";

        static async Task Main(string[] args)
        {
            using var bus = RabbitHutch.CreateBus(config.GetConnectionString("SOPRabbitMQ"));
            Console.WriteLine("Connected! Listening for NewVehicleMessage and NewOwnerMessages messages.");
            var vehiclesMessages = bus.PubSub.SubscribeAsync<NewVehicleMessage>(SUBSCRIBER_ID, HandleNewVehicleMessage);
            var ownerMessages = bus.PubSub.SubscribeAsync<NewOwnerMessage>(SUBSCRIBER_ID, HandleNewOwnerMessage);

            Task.WaitAll(vehiclesMessages, ownerMessages);

            Console.ReadKey(true);
        }

        private static void HandleNewVehicleMessage(NewVehicleMessage message)
        {
            var csv = $"{message.Registration},{message.Manufacturer},{message.ModelName},{message.Color},{message.Year},{message.ListedAtUtc:O}";
            Console.WriteLine(csv);
        }

        private static void HandleNewOwnerMessage(NewOwnerMessage message)
        {
            var csv = $"{message.Email}, {message.Surname}, {message.Name}, {message.VehicleRegistration}, {message.Birthday}, {message.ListedAtUtc}";
            Console.WriteLine(csv);
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
    }
}