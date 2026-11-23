using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Odoo.Request.Definitions;

/// <summary>
/// Options class usually contains parameters that are required.
/// </summary>
public class Options
{
    /// <summary>
    /// Gets or sets the Odoo URL.
    /// </summary>
    /// <example>https://example.com</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("https://example.com")]
    public string OdooUrl { get; set; }

    /// <summary>
    /// Gets or sets the username to authenticate with.
    /// </summary>
    /// <example>john.doe@example.com</example>
    [DefaultValue("john.doe@example.com")]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password to authenticate with.
    /// </summary>
    /// <example>Password!</example>
    [PasswordPropertyText]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the Odoo database.
    /// </summary>
    /// <example>MyDatabase</example>
    [DefaultValue("MyDatabase")]
    public string Database { get; set; }

    /// <summary>
    /// Gets or sets the compatibility flag for throwing an exception on failure.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowExceptionOnErrorResponse
    {
        get => this.ThrowErrorOnFailure;
        set => this.ThrowErrorOnFailure = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an error on failure.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Gets or sets the error message to use when the request fails.
    /// </summary>
    /// <example>Odoo request failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
