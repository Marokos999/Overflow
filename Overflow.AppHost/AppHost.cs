using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.4")
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithHttpEndpoint(port: 6001, targetPort: 8080, name: "http")
    .WithBindMount("keycloak-data", "/opt/keycloak/data")
    .WithArgs("start-dev");

var questionService = builder.AddProject<QuestionService>("question-svc")
    .WithReference(keycloak.GetEndpoint("http"))   
    .WaitFor(keycloak);

builder.Build().Run();