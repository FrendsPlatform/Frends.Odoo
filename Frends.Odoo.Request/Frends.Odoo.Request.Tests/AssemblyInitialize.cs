using System.IO;
using dotenv.net;
using NUnit.Framework;

/// <summary>
/// Initializes environment variables for test runs.
/// </summary>
[SetUpFixture]
public class AssemblyInitialize
{
    /// <summary>
    /// Loads environment variables before the tests run.
    /// </summary>
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var root = Directory.GetCurrentDirectory();
        var projDir = Directory.GetParent(root).Parent.Parent.FullName;
        DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { $"{projDir}/.env.local" }));
    }
}
