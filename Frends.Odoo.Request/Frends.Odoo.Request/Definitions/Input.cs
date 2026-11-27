using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Odoo.Request.Definitions;

/// <summary>
/// Input class contains parameters that are required.
/// </summary>
public class Input
{
    /// <summary>
    /// Model to call in the request. You can get a list of Odoo model by calling the "ir.model" model
    /// with the "search_read" method and the fields "name", "model" and "state".
    /// </summary>
    /// <example>res.partner</example>
    [DefaultValue("res.partner")]
    public string Model { get; set; }

    /// <summary>
    /// Method to execute in the request.
    /// </summary>
    /// <example>search_read</example>
    [DefaultValue("search_read")]
    public string Method { get; set; }

    /// <summary>
    /// Method arguments as JSON.
    /// </summary>
    /// <example>[[["is_company", "=", true]]]</example>
    [DisplayFormat(DataFormatString = "Json")]
    [DefaultValue("[]")]
    public string Args { get; set; }

    /// <summary>
    /// Method keyword arguments as JSON.
    /// </summary>
    /// <example>{ "fields": ["name", "email"], "limit": 5 }</example>
    [DisplayFormat(DataFormatString = "Json")]
    [DefaultValue("{}")]
    public string Kwargs { get; set; }
}
