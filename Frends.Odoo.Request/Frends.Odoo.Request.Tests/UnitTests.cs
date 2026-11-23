namespace Frends.Odoo.Request.Tests;

using System;
using System.Threading;
using System.Threading.Tasks;

using Frends.Odoo.Request.Definitions;
using NUnit.Framework;

[TestFixture]
internal class UnitTests
{
    private readonly string odooUrl = Environment.GetEnvironmentVariable("ODOO_URL");
    private readonly string username = Environment.GetEnvironmentVariable("ODOO_USERNAME");
    private readonly string password = Environment.GetEnvironmentVariable("ODOO_PASSWORD");
    private readonly string database = Environment.GetEnvironmentVariable("ODOO_DATABASE");

    /// <summary>
    /// Confirms invalid credentials are returned as an error result.
    /// </summary>
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
        Assert.IsTrue(result.Error.Message.StartsWith("Authentication failed. Odoo's response: Access Denied"), result.Error.Message);
        Assert.IsNull(result.Data);
    }

    /// <summary>
    /// Confirms an invalid database is returned as an error result.
    /// </summary>
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
        Assert.IsTrue(result.Error.Message.Contains("database \"invalid\" does not exist"), result.Error.Message);
        Assert.IsNull(result.Data);
    }

    /// <summary>
    /// Confirms failures can be configured to throw exceptions.
    /// </summary>
    [Test]
    public void Request_InvalidCredentials_ThrowsErrorAsException()
    {
        // Arrange
        var input = GetDefaultInput();
        var options = GetDefaultOptions();
        options.Password = "invalid";
        options.ThrowErrorOnFailure = true;

        // Act
        var exception = Assert.ThrowsAsync<Exception>(
            async () => await Odoo.Request(input, options, CancellationToken.None));

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual("Authentication failed. Odoo's response: Access Denied", exception.Message);
    }

    /// <summary>
    /// Confirms invalid request arguments are returned as an error result.
    /// </summary>
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
        Assert.IsTrue(result.Error.Message.Contains("builtins.TypeError"), result.Error.Message);
        Assert.IsNull(result.Data);
    }

    /// <summary>
    /// Confirms error results include additional information and the custom message.
    /// </summary>
    [Test]
    public async Task Request_InvalidUrl_ErrorHasAdditionalInfoAndCustomMessage()
    {
        var input = GetDefaultInput();
        var options = GetDefaultOptions();
        options.OdooUrl = "invalid-url";
        options.ErrorMessageOnFailure = "Unable to contact Odoo";

        var result = await Odoo.Request(input, options, CancellationToken.None);

        Assert.IsFalse(result.Success);
        Assert.IsTrue(result.Error.Message.StartsWith("Unable to contact Odoo:"), result.Error.Message);
        Assert.IsNotNull(result.Error.AdditionalInfo);
    }

    /// <summary>
    /// Confirms a valid request returns data successfully.
    /// </summary>
    [Test]
    public async Task Request_CorrectRequest_Success()
    {
        // Arrange
        var input = GetDefaultInput();
        var options = GetDefaultOptions();

        // Act
        var result = await Odoo.Request(input, options, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.Success, result.Error?.Message);
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
            ThrowErrorOnFailure = false,
        };
}
