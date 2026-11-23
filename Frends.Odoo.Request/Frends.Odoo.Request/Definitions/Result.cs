namespace Frends.Odoo.Request.Definitions;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets or initializes a value indicating whether the operation was successful.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; init; }

    /// <summary>
    /// Gets or initializes the error message if the operation failed.
    /// </summary>
    /// <example>Authentication failed: Access Denied</example>
    public string Error { get; init; }

    /// <summary>
    /// Gets or initializes the data returned by the operation in JToken format.
    /// Dot notation can be used to access field values.
    /// </summary>
    /// <example>[{ "id": 7, "name": "Azure Interior", "email": "info@azureinterior.com" }]</example>
    public dynamic Data { get; init; }
}
