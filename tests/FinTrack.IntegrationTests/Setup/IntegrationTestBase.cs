namespace FinTrack.IntegrationTests.Setup;

/// <summary>
/// Classe base para testes de integração, responsável por inicializar o client HTTP e 
/// o ambiente de testes utilizando o <see cref="CustomWebApplicationFactory"/>.
/// </summary>
public class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>
{
    /// <summary>
    /// Instância da factory personalizada que configura o ambiente de teste com Testcontainers.
    /// </summary>
    protected readonly CustomWebApplicationFactory _factory;

    /// <summary>
    /// Cliente HTTP configurado para executar requisições na aplicação.
    /// </summary>
    protected readonly HttpClient _client;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="IntegrationTestBase"/> com a factory injetada.
    /// </summary>
    /// <param name="factory">Factory responsável por configurar o ambiente da aplicação para os testes.</param>
    public IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
}