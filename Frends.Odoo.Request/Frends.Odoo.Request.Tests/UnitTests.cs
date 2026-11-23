using System;
using System.Threading;
using System.Threading.Tasks;
using Frends.Odoo.Request.Definitions;
using NUnit.Framework;

namespace Frends.Odoo.Request.Tests;

[TestFixture]
internal class UnitTests
{
    [Test]
    public async Task Request_InvalidCredentials_Error()
    {
        // Arrange
        var input = GetDefaultInput();
        var connection = GetDefaultConnection();
        var options = GetDefaultOptions();
        connection.Password = "invalid";

        // Act
        var result = await Odoo.Request(input, connection, options, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.StartWith("Authentication failed. Odoo's response: Access Denied"));
        Assert.That((object)result.Data, Is.Null);
    }

    [Test]
    public async Task Request_InvalidDatabase_Error()
    {
        // Arrange
        var input = GetDefaultInput();
        var connection = GetDefaultConnection();
        var options = GetDefaultOptions();
        connection.Database = "invalid";

        // Act
        var result = await Odoo.Request(input, connection, options, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.Contain("Database not found"));
        Assert.That((object)result.Data, Is.Null);
    }

    [Test]
    public async Task Request_InvalidCredentials_ThrowsErrorAsException()
    {
        // Arrange
        var input = GetDefaultInput();
        var connection = GetDefaultConnection();
        var options = GetDefaultOptions();
        connection.Password = "invalid";
        options.ThrowErrorOnFailure = true;

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(
            async () => await Odoo.Request(input, connection, options, CancellationToken.None));

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Authentication failed. Odoo's response: Access Denied"));
    }

    [Test]
    public async Task Request_InvalidRequest_Error()
    {
        // Arrange
        var input = GetDefaultInput();
        var connection = GetDefaultConnection();
        var options = GetDefaultOptions();
        input.Kwargs = "{ 'fields': ['name', 'email'], 'limit': 5, 'invalid': 'invalid' }";

        // Act
        var result = await Odoo.Request(input, connection, options, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error.Message, Does.Contain("builtins.TypeError"));
        Assert.That((object)result.Data, Is.Null);
    }

    [Test]
    public async Task Request_CorrectRequest_Success()
    {
        // Arrange
        var input = GetDefaultInput();
        var connection = GetDefaultConnection();
        var options = GetDefaultOptions();

        // Act
        var result = await Odoo.Request(input, connection, options, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.True, result.Error?.Message);
        Assert.That(result.Error, Is.Null);
        Assert.That((object)result.Data, Is.Not.Null);
        Assert.That((int)result.Data.Count, Is.EqualTo(5));
    }

    private static Input GetDefaultInput() =>
        new()
        {
            Model = "res.partner",
            Method = "search_read",
            Args = "[]",
            Kwargs = "{ 'fields': ['name', 'email'], 'limit': 5 }",
        };

    private static Connection GetDefaultConnection() =>
        new()
        {
            OdooUrl = AssemblyInitialize.OdooUrl,
            Username = AssemblyInitialize.Username,
            Password = AssemblyInitialize.Password,
            Database = AssemblyInitialize.Database,
        };

    private static Options GetDefaultOptions() =>
        new()
        {
            ThrowErrorOnFailure = false,
        };
}
