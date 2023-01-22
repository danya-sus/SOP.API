
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using SOP.AgeServer;
using SOP.Messages.Messages;

namespace SOP.AuditLog
{
    class Program
    {
        private static Ager.AgerClient _grpcClient;
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private const string SUBSCRIBER_ID = "SOP.AuditLog";
        private static IBus _bus;

        static async Task Main(string[] args)
        {
            _bus = RabbitHutch.CreateBus(config.GetConnectionString("SOPRabbitMQ"));
            Console.WriteLine("Connected! Listening for NewVehicleMessage and NewOwnerMessages messages.");
            var vehiclesMessages = _bus.PubSub.SubscribeAsync<NewVehicleMessage>(SUBSCRIBER_ID, HandleNewVehicleMessage);
            var ownerMessages = _bus.PubSub.SubscribeAsync<NewOwnerMessage>(SUBSCRIBER_ID, HandleNewOwnerMessage);

            var grpcAddress = "http://localhost:5052";
            using var channel = GrpcChannel.ForAddress(grpcAddress);
            _grpcClient = new Ager.AgerClient(channel);
            Console.WriteLine($"Connected to gRPC on {grpcAddress}");

            Task.WaitAll(vehiclesMessages, ownerMessages);

            Console.ReadKey(true);
        }

        private static void HandleNewVehicleMessage(NewVehicleMessage message)
        {
            var csv = $"{message.Registration},{message.Manufacturer},{message.ModelName},{message.Color},{message.Year},{message.ListedAtUtc:O}";
            Console.WriteLine(csv);
        }

        private static async Task HandleNewOwnerMessage(NewOwnerMessage message)
        {
            var ageRequest = new AgeRequest
            {
                Email = message.Email,
                Name = message.Name,
                Surname = message.Surname,
                Birthday = message.Birthday.Replace(".", "-"),
                VehicleRegistration = message.VehicleRegistration
            };

            var ageReply = await _grpcClient.GetAgeAsync(ageRequest);

            var newOwnerAgeMessage = new NewOwnerAgeMessage(message, ageReply.Years, ageReply.Months, ageReply.Days);
            await _bus.PubSub.PublishAsync(newOwnerAgeMessage);
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
