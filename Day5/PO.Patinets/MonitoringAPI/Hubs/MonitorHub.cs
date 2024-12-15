using Microsoft.AspNetCore.SignalR;

namespace MonitoringAPI.Hubs;

public class MonitorHub : Hub
{

    public Task SubscibeToPatientMonitor(int patientId)
    {
        //Clients.Caller.SendAsync("SubscribeToPatientMonitor", patientId);
         return Groups.AddToGroupAsync(Context.ConnectionId, $"patient:{patientId}");
    }

    public Task UnsubscribeFromPatientMonitor(int patientId)
    {
        //Clients.Caller.SendAsync("UnsubscribeFromPatientMonitor", patientId);
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, $"patient:{patientId}");
    }
}
