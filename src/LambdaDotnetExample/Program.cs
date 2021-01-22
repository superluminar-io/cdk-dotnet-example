using Amazon.CDK;

namespace LambdaDotnetExample
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new LambdaDotnetExampleStack(app, "LambdaDotnetExampleStack");
            app.Synth();
        }
    }
}
