var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ClientApplcationBFF>("clientapplcationbff");

builder.AddProject<Projects.MonitoringAPI>("monitoringapi");

builder.AddProject<Projects.Patients_CatalogAPI>("patients-catalogapi");

builder.Build().Run();
