

using Grpc.Net.Client;
using SOP.AgeServer;

using var channel = GrpcChannel.ForAddress("https://localhost:7052");
var grpcClient = new Ager.AgerClient(channel);
Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-C to quit).");
while (true)
{
    Console.ReadKey(true);
    var request = new AgeRequest
    {
        Email = "danya.sus@yandex.ru",
        Name = "Danil",
        Surname = "Suslov",
        Birthday = "12-08-2000",
        VehicleRegistration = "NEW123Z"
    };

    var reply = grpcClient.GetAge(request);
    Console.WriteLine($"You are {reply.Years}, {reply.Months} months and {reply.Days} days ago");
}