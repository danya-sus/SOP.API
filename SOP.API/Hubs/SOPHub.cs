using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SOP.API.Hubs
{
    public class SOPHub : Hub
    {
        public async Task NotifyWebUsers(string user, string message)
        {
            await Clients.All.SendAsync("DisplayNotification", user, message);
        }
    }
}
