namespace Patients.EventBus.Rabbit;

public class RabbitMQSettings
{
    public string Hostname { get; set; }
    public string ExchangeName { get; set; }
    public string Password { get;  set; }
    public string UserName { get; set; }

    public int Port { get; set; }
}

