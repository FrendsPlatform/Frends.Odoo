namespace Frends.Odoo.Request.Definitions;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="success">Whether the operation was successful.</param>
    /// <param name="data">Data returned by the operation.</param>
    /// <param name="error">Error details when the operation failed.</param>
    public Result(bool success, dynamic data = null, Error error = null)
    {
        this.Success = success;
        this.Data = data;
        this.Error = error;
    }

    /// <summary>
    /// True if the operation was successful. False if the operation failed.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; }

    /// <summary>
    /// Error details if the operation failed.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error Error { get; }

    /// <summary>
    /// Data returned by the operation in JToken format.
    /// You can use dot notation to access the field values.
    /// </summary>
    /// <example>[ { "field1": "value1" }, { "field1": "value2" } ]</example>
    public dynamic Data { get; }
}
