using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.4")
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithEnvironment("KC_LOG_LEVEL", "INFO")
    .WithHttpEndpoint(port: 6001, targetPort: 8080, name: "http")
    .WithBindMount("keycloak-data", "/opt/keycloak/data")
    .WithArgs("start-dev");

var postgres = builder.AddPostgres("postgres", port: 5432)
    .WithDataVolume("postgres-data")
    .WithPgAdmin();

var questionDb = postgres.AddDatabase("question-db");

var questionService = builder.AddProject<QuestionService>("question-svc")
    .WithReference(keycloak.GetEndpoint("http"))   
    .WithReference(questionDb)
    .WaitFor(keycloak)
    .WaitFor(postgres);

builder.Build().Run();