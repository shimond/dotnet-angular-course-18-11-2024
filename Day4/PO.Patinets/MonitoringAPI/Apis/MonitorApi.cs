using Microsoft.AspNetCore.Http.HttpResults;
using MonitoringAPI.Contracts;
using MonitoringAPI.Models;

namespace MonitoringAPI.Apis
{
    public static class MonitorApi
    {
        public static void UseMonitorApi(this IEndpointRouteBuilder endpoints)
        {
            var monitorGroup = endpoints.MapGroup("monitor-patient");
            monitorGroup.MapPost("", SaveMonitor);
            monitorGroup.MapGet("{id}", GetPatientMonitor);
        }


        async static Task<Results<Ok<MonitorRequest>, NotFound>> GetPatientMonitor(int id, IMonitorService service)
        {
            var res = await service.GetByPatientIdAsync(id);
            if (res is not null)
            {
                return TypedResults.Ok(res);
            }

            return TypedResults.NotFound();

        }




        async static Task<Ok<MonitorRequest>> SaveMonitor(MonitorRequest request, IMonitorService service)
        {
            await service.SavePatientMonitoringAsync(request);
            return TypedResults.Ok(request);

        }
    }
}
