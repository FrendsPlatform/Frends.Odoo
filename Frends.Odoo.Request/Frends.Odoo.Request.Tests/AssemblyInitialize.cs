using System.IO;
using dotenv.net;
using NUnit.Framework;

[SetUpFixture]
public class AssemblyInitialize
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var root = Directory.GetCurrentDirectory();
        var projDir = Directory.GetParent(root).Parent.Parent.FullName;
        DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { $"{projDir}/.env.local" }));
    }
}