using System;
using System.IO;
using System.Threading.Tasks;
using dotenv.net;

namespace Frends.Odoo.Request.Tests;

using Frends.Odoo.Request.Definitions;
using NUnit.Framework;

[TestFixture]
internal class UnitTests
{
    private readonly string _odooUrl = Environment.GetEnvironmentVariable("ODOO_URL");
    private readonly string _username = Environment.GetEnvironmentVariable("ODOO_USERNAME");
    private readonly string _password = Environment.GetEnvironmentVariable("ODOO_PASSWORD");
    private readonly string _database = Environment.GetEnvironmentVariable("ODOO_DATABASE");

    [Test]
    public async Task Request_InvalidCredentials_Error()
    {
        // Arrange
        var input = GetDefaultInput();
        var options = GetDefaultOptions();
        options.Password = "invalid";

        // Act
        var result = await Odoo.Request(input, options, default);

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
        var result = await Odoo.Request(input, options, default);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsTrue(result.Error.Contains("database \"invalid\" does not exist"), result.Error);
        Assert.IsNull(result.Data);
    }

    [Test]
    public async Task Request_InvalidCredentials_ThrowsErrorAsException()
    {
        // Arrange
        var input = GetDefaultInput();
        var options = GetDefaultOptions();
        options.Password = "invalid";
        options.ThrowExceptionOnErrorResponse = true;

        // Act
        Exception exception = null;
        try
        {
            await Odoo.Request(input, options, default);
        }
        catch (Exception e)
        {
            exception = e;
        }

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
        var result = await Odoo.Request(input, options, default);

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
        var result = await Odoo.Request(input, options, default);

        // Assert
        Assert.IsTrue(result.Success, result.Error);
        Assert.IsNull(result.Error);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(5, result.Data.Count);
    }

    private Input GetDefaultInput()
    {
        return new Input
        {
            Model = "res.partner",
            Method = "search_read",
            Args = "[]",
            Kwargs = "{ 'fields': ['name', 'email'], 'limit': 5 }",
        };
    }

    private Options GetDefaultOptions()
    {
        return new Options
        {
            OdooUrl = _odooUrl,
            Username = _username,
            Password = _password,
            Database = _database,
            ThrowExceptionOnErrorResponse = false,
        };
    }
}
