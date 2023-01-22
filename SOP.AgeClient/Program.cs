

using EasyNetQ;
using Grpc.Net.Client;
using SOP.AgeServer;
using SOP.Messages.Messages;

namespace SOP.AgeClient
{
    class Program
    {
        private static Ager.AgerClient _grpcClient;
        private const string SUBSCRIBER_ID = "SOP.AgeClient";
        private static IBus _bus;

        static async Task Main(string[] args)
        {
            Console.WriteLine("SOP.AgerClient starting...");

            var amqp = "amqp://user:rabbitmq@localhost:5672";
            _bus = RabbitHutch.CreateBus(amqp);

            Console.WriteLine("Connected to bus. Listening for newOwnerMessage...");

            var grpcAddress = "http://localhost:5052";
            using var channel = GrpcChannel.ForAddress(grpcAddress);
            _grpcClient = new Ager.AgerClient(channel);

            Console.WriteLine($"Connected to gRPC on {grpcAddress}");

            await _bus.PubSub.SubscribeAsync<NewOwnerMessage>(SUBSCRIBER_ID, HandleNewOwnerMessage);

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static async Task HandleNewOwnerMessage(NewOwnerMessage message)
        {
            Console.WriteLine($"New owner: {message.Email}");
            var ageRequest = new AgeRequest
            {
                Email = message.Email,
                Name = message.Name,
                Surname = message.Surname,
                Birthday = message.Birthday.ToString(),
                VehicleRegistration = message.VehicleRegistration
            };

            var ageReply = await _grpcClient.GetAgeAsync(ageRequest);

            var newOwnerAgeMessage = new NewOwnerAgeMessage(message, ageReply.Years, ageReply.Months, ageReply.Days);
            await _bus.PubSub.PublishAsync(newOwnerAgeMessage);
        }
    }
}