using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Odoo.Request.Definitions;

/// <summary>
/// Connection parameters.
/// </summary>
public class Connection
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
}
