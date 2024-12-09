using MonitoringAPI.Models;

namespace MonitoringAPI.Contracts;

public interface IMonitorService
{
    Task<MonitorRequest?> GetByPatientIdAsync(int patientId);
    Task SavePatientMonitoringAsync(MonitorRequest request);
}
