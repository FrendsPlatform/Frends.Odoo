using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using NUnit.Framework;

namespace Frends.Odoo.Request.Tests;

[SetUpFixture]
public class AssemblyInitialize
{
    public const string Database = "odoo_test";
    public const string Username = "admin";
    public const string Password = "admin";

    private const string DbAlias = "db";
    private const string DbUser = "odoo";
    private const string DbPassword = "odoo";

    private INetwork network;
    private IContainer postgres;
    private IContainer odoo;

    public static string OdooUrl { get; private set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        network = new NetworkBuilder().Build();
        await network.CreateAsync();

        postgres = new ContainerBuilder("postgres:15")
            .WithNetwork(network)
            .WithNetworkAliases(DbAlias)
            .WithEnvironment("POSTGRES_DB", "postgres")
            .WithEnvironment("POSTGRES_USER", DbUser)
            .WithEnvironment("POSTGRES_PASSWORD", DbPassword)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("database system is ready to accept connections"))
            .Build();
        await postgres.StartAsync();

        // Initializes the database with base module and demo data, then serves requests.
        odoo = new ContainerBuilder("odoo:17.0")
            .WithNetwork(network)
            .WithEnvironment("HOST", DbAlias)
            .WithEnvironment("USER", DbUser)
            .WithEnvironment("PASSWORD", DbPassword)
            .WithCommand("-d", Database, "-i", "base")
            .WithPortBinding(8069, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("Modules loaded\\."))
            .Build();
        await odoo.StartAsync();

        OdooUrl = $"http://{odoo.Hostname}:{odoo.GetMappedPublicPort(8069)}";
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (odoo != null) await odoo.DisposeAsync();
        if (postgres != null) await postgres.DisposeAsync();
        if (network != null) await network.DisposeAsync();
    }
}
