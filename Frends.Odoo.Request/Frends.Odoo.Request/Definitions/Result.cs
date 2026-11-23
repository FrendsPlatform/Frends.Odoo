namespace Frends.Odoo.Request.Definitions;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    /// <summary>
    /// True if the operation was successful. False if the operation failed.
    /// If optional parameter ThrowErrorOnFailure is set to true, the task throws an exception if the
    /// response from Odoo contains an error.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; init; }

    /// <summary>
    /// Error that occurred during task execution.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error Error { get; init; }

    /// <summary>
    /// Data returned by the operation in JToken format.
    /// You can use dot notation to access the field values.
    /// </summary>
    /// <example>[{ "id": 7, "name": "Azure Interior", "email": "info@azureinterior.com" }]</example>
    public dynamic Data { get; init; }
}
