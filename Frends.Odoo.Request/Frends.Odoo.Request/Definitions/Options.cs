namespace Frends.Odoo.Request.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Options class usually contains parameters that are required.
/// </summary>
public class Options
{
    /// <summary>
    /// Odoo URL.
    /// </summary>
    /// <example>https://example.com</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("https://example.com")]
    public string OdooUrl { get; set; }

    /// <summary>
    /// Username to authenticate with.
    /// </summary>
    /// <example>john.doe@example.com</example>
    [DefaultValue("john.doe@example.com")]
    public string Username { get; set; }

    /// <summary>
    /// Password to authenticate with.
    /// </summary>
    /// <example>Password!</example>
    [PasswordPropertyText]
    public string Password { get; set; }

    /// <summary>
    /// Odoo database.
    /// </summary>
    /// <example>MyDatabase</example>
    [DefaultValue("MyDatabase")]
    public string Database { get; set; }

    /// <summary>
    /// Whether to throw an error when the task fails.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Overrides the error message when the task fails.
    /// </summary>
    /// <example>Odoo request failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
