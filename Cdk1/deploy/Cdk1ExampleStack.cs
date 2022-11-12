using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace Deploy
{
    public class Cdk1ExampleStack : Stack
    {
        internal Cdk1ExampleStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var lambda1Function = new Function(this, "Lambda1", new FunctionProps
            {
                FunctionName = "Lambda1",
                Runtime = Runtime.DOTNET_6,
                MemorySize = 128,
                Architecture = Architecture.X86_64,
                Tracing = Tracing.ACTIVE,
                Code = Code.FromAsset("../src/Lambda1/bin/Release/net6.0/publish"),
                Handler = "Lambda1::Lambda1.Function::FunctionHandler"
            });

            var lambda2Function = new Function(this, "Lambda2", new FunctionProps
            {
                FunctionName = "Lambda2",
                Runtime = Runtime.DOTNET_6,
                MemorySize = 128,
                Architecture = Architecture.X86_64,
                Tracing = Tracing.ACTIVE,
                Code = Code.FromAsset("../src/Lambda2/bin/Release/net6.0/publish"),
                Handler = "Lambda2::Lambda2.Function::FunctionHandler"
            });

            lambda2Function.GrantInvoke(lambda1Function);
        }
    }
}