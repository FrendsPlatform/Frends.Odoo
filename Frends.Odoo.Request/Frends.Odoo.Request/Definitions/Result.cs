namespace Frends.Odoo.Request.Definitions;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    /// <summary>
    /// True if the operation was successful. False if the operation failed.
    /// If optional parameter ThrowExceptionOnErrorResponse is set to true, the task throws an exception if the
    /// response from Odoo contains an error.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; init; }

    /// <summary>
    /// Error message if the operation failed.
    /// </summary>
    /// <example>Error message</example>
    public string Error { get; init; }

    /// <summary>
    /// Data returned by the operation in JToken format.
    /// You can use dot notation to access the field values.
    /// </summary>
    /// <example>[ { "field1": "value1" }, { "field1": "value2" } ]</example>
    public dynamic Data { get; init; }
}
