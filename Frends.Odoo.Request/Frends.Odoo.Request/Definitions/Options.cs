using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Odoo.Request.Definitions;

/// <summary>
/// Options class usually contains parameters that are required.
/// </summary>
public class Options
{
    /// <summary>
    /// If true, the task throws an exception on failure. If false, the task returns the error in the result.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Message to use as the error message when the request fails.
    /// </summary>
    /// <example>Odoo request failed</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; set; } = string.Empty;
}
