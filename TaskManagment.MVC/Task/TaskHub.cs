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
        private async System.Threading.Tasks.Task SendTaskToManagersInternal(string method, object task)
        {
            await Clients.Group(DefaultRoles.Manager).SendAsync(method, task);
        }

        public async System.Threading.Tasks.Task SendTaskToManagers(object task)
        {
            await SendTaskToManagersInternal("addtask", task);
        }

        public async System.Threading.Tasks.Task EditTaskToManagers(object task)
        {
            await SendTaskToManagersInternal("edittask", task);
        }

        public async System.Threading.Tasks.Task DeleteTaskToManagers(object task)
        {
            await SendTaskToManagersInternal("deletetask", task);
        }

    }
}
