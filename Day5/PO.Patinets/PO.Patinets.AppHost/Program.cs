var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("myRedis");
var sql = builder.AddSqlServer("myPatientsDb")    
                        .WithLifetime(ContainerLifetime.Persistent);

var rabbit = builder.AddRabbitMQ("rabbitPOClient");

var monitor = builder.AddProject<Projects.MonitoringAPI>("monitoringapi")
    .WithReference(rabbit)
    .WithReference(redis)
    .WaitFor(redis).WaitFor(rabbit);

var catalog = builder.AddProject<Projects.Patients_CatalogAPI>("patients-catalogapi")
    .WithReference(sql)
    .WaitFor(sql);


builder.AddProject<Projects.ClientApplcationBFF>("clientapplcationbff")
        .WithReference(catalog)
        .WithReference(monitor);

builder.Build().Run();

