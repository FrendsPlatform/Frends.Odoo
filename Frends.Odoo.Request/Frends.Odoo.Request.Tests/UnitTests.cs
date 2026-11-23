using System;
using System.Threading;
using System.Threading.Tasks;
using Frends.Odoo.Request.Definitions;
using NUnit.Framework;

namespace Frends.Odoo.Request.Tests;

[TestFixture]
internal class UnitTests
{
    private readonly string odooUrl = Environment.GetEnvironmentVariable("ODOO_URL");
    private readonly string username = Environment.GetEnvironmentVariable("ODOO_USERNAME");
    private readonly string password = Environment.GetEnvironmentVariable("ODOO_PASSWORD");
    private readonly string database = Environment.GetEnvironmentVariable("ODOO_DATABASE");

    [Test]
    public void Options_ThrowExceptionOnErrorResponse_ControlsThrowErrorOnFailure()
    {
        var options = new Options();

        Assert.IsTrue(options.ThrowErrorOnFailure);
        Assert.IsTrue(options.ThrowExceptionOnErrorResponse);
        options.ThrowExceptionOnErrorResponse = false;
        Assert.IsFalse(options.ThrowErrorOnFailure);
        options.ThrowExceptionOnErrorResponse = true;
        Assert.IsTrue(options.ThrowErrorOnFailure);
        options.ThrowErrorOnFailure = false;
        Assert.IsFalse(options.ThrowExceptionOnErrorResponse);
    }

    [Test]
    public async Task Request_InvalidCredentials_Error()
    {
        // Arrange
        var input = GetDefaultInput();
        var options = GetDefaultOptions();
        options.Password = "invalid";

        // Act
        var result = await Odoo.Request(input, options, CancellationToken.None);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsTrue(result.Error.StartsWith("Authentication failed. Odoo's response: Access Denied"), result.Error);
        Assert.IsNull(result.Data);
    }

    [Test]
    public async Task Request_InvalidDatabase_Error()
    {
        // Arrange
        var input = GetDefaultInput();
        var options = GetDefaultOptions();
        options.Database = "invalid";

        // Act
        var result = await Odoo.Request(input, options, CancellationToken.None);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsTrue(result.Error.Contains("database \"invalid\" does not exist"), result.Error);
        Assert.IsNull(result.Data);
    }

    [Test]
    public void Request_InvalidCredentials_ThrowsErrorAsException()
    {
        // Arrange
        var input = GetDefaultInput();
        var options = GetDefaultOptions();
        options.Password = "invalid";
        options.ThrowExceptionOnErrorResponse = true;

        // Act
        var exception = Assert.ThrowsAsync<Exception>(
            async () => await Odoo.Request(input, options, CancellationToken.None));

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual("Authentication failed. Odoo's response: Access Denied", exception.Message);
    }

    [Test]
    public async Task Request_InvalidRequest_Error()
    {
        // Arrange
        var input = GetDefaultInput();
        var options = GetDefaultOptions();
        input.Kwargs = "{ 'fields': ['name', 'email'], 'limit': 5, 'invalid': 'invalid' }";

        // Act
        var result = await Odoo.Request(input, options, CancellationToken.None);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.IsTrue(result.Error.Contains("builtins.TypeError"), result.Error);
        Assert.IsNull(result.Data);
    }

    [Test]
    public async Task Request_CorrectRequest_Success()
    {
        // Arrange
        var input = GetDefaultInput();
        var options = GetDefaultOptions();

        // Act
        var result = await Odoo.Request(input, options, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.Success, result.Error);
        Assert.IsNull(result.Error);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(5, result.Data.Count);
    }

    private static Input GetDefaultInput() =>
        new()
        {
            Model = "res.partner",
            Method = "search_read",
            Args = "[]",
            Kwargs = "{ 'fields': ['name', 'email'], 'limit': 5 }",
        };

    private Options GetDefaultOptions() =>
        new()
        {
            OdooUrl = odooUrl,
            Username = username,
            Password = password,
            Database = database,
            ThrowExceptionOnErrorResponse = false,
        };
}
