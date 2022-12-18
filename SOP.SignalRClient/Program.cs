

using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using SOP.Models.Entities;

public class Program
{
    const string SIGNALR_HUB_URL = "http://localhost:5034/hub";
    private static HubConnection hub;

    static async Task Main(string[] args)
    {
        hub = new HubConnectionBuilder().WithUrl(SIGNALR_HUB_URL).Build();
        await hub.StartAsync();
        Console.WriteLine("Hub started!");
        Console.WriteLine("Press any key...");
        var i = 0;
        while (true)
        {
            var input = Console.ReadLine();

            var owner = new
            {
                email = "testing@yandex.ru",
                name = "Ivan",
                surname = "Ivanov",
                birthday = "01-01-2001",
                registrationVehicle = "NEW123A"
            };

            var ownerJson = JObject.FromObject(owner).ToString();

            var vehicle = new
            {
                registration = "NEW123A",
                manufacturerName = "audi",
                modelName = "audi-80",
                color = "Black",
                year = 2001,
                currencyCode = "RUB",
                price = 200000
            };

            var vehicleJson = JObject.FromObject(vehicle).ToString();

            var message = $"Message #{i++} from SOP.SignalRClient {input}";
            await hub.SendAsync("NotifyWebUsers", "SOP.SignalRClient", ownerJson);
            Console.WriteLine($"Sent: {message}");
        }
    }
}