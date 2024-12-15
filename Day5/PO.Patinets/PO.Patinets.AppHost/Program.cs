var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ClientApplcationBFF>("clientapplcationbff");

builder.Build().Run();
