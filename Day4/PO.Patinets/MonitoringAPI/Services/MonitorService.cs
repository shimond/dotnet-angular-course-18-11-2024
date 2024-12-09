using MonitoringAPI.Contracts;
using MonitoringAPI.Models;
using Patients.EventBus.Abstraction;
using StackExchange.Redis;
using System.Text.Json;

namespace MonitoringAPI.Services;

public class MonitorService : IMonitorService
{
    private readonly IPOEventBus _eventBus;
    private readonly IDatabase _redisDb;

    public MonitorService(IConnectionMultiplexer redisConnection, IPOEventBus eventBus)
    {
        _eventBus = eventBus;
        _redisDb = redisConnection.GetDatabase();
    }

    public async Task<MonitorRequest?> GetByPatientIdAsync(int patientId)
    {
        string key = GetRedisKey(patientId);
        var data = await _redisDb.StringGetAsync(key);

        if (data.IsNullOrEmpty)
            return null;

        // Deserialize the JSON data back into a MonitorRequest object
        return JsonSerializer.Deserialize<MonitorRequest>(data);
    }

    public async Task SavePatientMonitoringAsync(MonitorRequest request)
    {
        string key = GetRedisKey(request.PatientId);

        // Serialize the MonitorRequest object to JSON
        string value = JsonSerializer.Serialize(request);

        await _redisDb.StringSetAsync(key, value);
        await _eventBus.Publish(new PatientDataMsg { 
            PatientId = request.PatientId,
            Fever = request.Fever,
            MsgTime = DateTime.Now
        });
    }

    private static string GetRedisKey(int patientId)
    {
        return $"patient:{patientId}";
    }
}

