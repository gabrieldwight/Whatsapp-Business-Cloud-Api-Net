using Microsoft.AspNetCore.SignalR;

namespace WhatsAppBusinessCloudAPI.Web.Hubs
{
    public class WhatsAppMessagesHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
