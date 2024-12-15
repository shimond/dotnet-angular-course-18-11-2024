using Patients.EventBus.Abstraction;

namespace MonitoringAPI;

public class PatientDataMsg : IntegrationMsg
{
    public DateTime MsgTime { get; set; }
    public int PatientId { get; set; }
    public double Fever { get; set; }
}
