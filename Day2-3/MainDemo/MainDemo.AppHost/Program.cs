var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("cacheData");
var catalogProject = builder.AddProject<Projects.CatalogApi>("catalogapi");

var currencyProject = builder.AddProject<Projects.CurrencyApi>("currencyapi")
    .WithReference(redis);

var bff = builder.AddProject<Projects.BFF>("bff")
        .WithReference(catalogProject)
        .WithReference(currencyProject);
builder.Build().Run();
