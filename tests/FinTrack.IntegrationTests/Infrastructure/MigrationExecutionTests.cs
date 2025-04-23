using Dapper;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using FinTrack.IntegrationTests.Setup;

namespace FinTrack.IntegrationTests.Infrastructure;

public class MigrationExecutionTests : IntegrationTestBase
{
    public MigrationExecutionTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact(DisplayName = "Deve aplicar as migrations e criar as tabelas no banco")]
    public async Task Should_Apply_Migrations_And_Create_Tables()
    {
        // Arrange
        using var connection = new SqlConnection(_factory.ConnectionString);
        await connection.OpenAsync();

        // Act
        var appliedMigrations = await connection.QueryAsync<string>(
            "SELECT [MigrationId] FROM [__EFMigrationsHistory];");

        // Assert
        appliedMigrations.Should().NotBeEmpty("Porque a aplicação deve aplicar pelo menos uma migration válida ao inicializar o banco");
    }
}