using Microsoft.AspNetCore.SignalR;
using DBAccess;
using Project_s_classes;

namespace OnlineSupport.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task<bool> IsAdminOnline()
        {
            return Admin.IsAnyAdminOnline();
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
            {
                Admin.SetOnlineStatus(username, true);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
            {
                Admin.SetOnlineStatus(username, false);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}