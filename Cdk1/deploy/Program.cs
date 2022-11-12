using Amazon.CDK;

namespace Deploy
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new Cdk1ExampleStack(app, "DeployStack", new StackProps());
            app.Synth();
        }
    }
}
