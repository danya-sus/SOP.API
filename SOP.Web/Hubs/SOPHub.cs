using Microsoft.AspNetCore.SignalR;

namespace SOP.Web.Hubs
{
    public class SOPHub : Hub
    {
        public async Task NotifyWebUsers(string user, string message)
        {
            await Clients.All.SendAsync("DisplayNotification", user, message);
        }
    }
}
