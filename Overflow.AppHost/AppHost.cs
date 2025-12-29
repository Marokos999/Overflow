var builder = DistributedApplication.CreateBuilder(args);

builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.4")
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithHttpEndpoint(port: 6001, targetPort: 8080)
    .WithArgs("start-dev");

builder.Build().Run();