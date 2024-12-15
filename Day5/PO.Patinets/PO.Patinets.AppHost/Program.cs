var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ClientApplcationBFF>("clientapplcationbff");

builder.AddProject<Projects.MonitoringAPI>("monitoringapi");

builder.Build().Run();
