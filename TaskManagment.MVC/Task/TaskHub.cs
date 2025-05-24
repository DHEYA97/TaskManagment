using Microsoft.AspNetCore.SignalR;
using TaskManagment.Core.Abstractions;


namespace TaskManagment.Mvc.Task
{
    public class TaskHub : Hub
    {
        public override async System.Threading.Tasks.Task OnConnectedAsync()
        {
            if (Context.User.IsInRole(DefaultRoles.Manager))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, DefaultRoles.Manager);
            }

            await base.OnConnectedAsync();
        }
        public async System.Threading.Tasks.Task SendTaskToManagers(object task)
        {
            await Clients.Group(DefaultRoles.Manager).SendAsync("addtask", task);
        }
    }
}
