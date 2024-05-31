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
    /// If true, the task throws an exception if the response from Odoo contains an error.
    /// If false, the task returns the error message in the result.
    /// </summary>
    /// <example>Example of the parameter value</example>
    [DefaultValue(false)]
    public bool ThrowExceptionOnErrorResponse { get; set; }
}