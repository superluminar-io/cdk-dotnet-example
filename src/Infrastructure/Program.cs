using Amazon.CDK;

namespace Infrastructure
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new InfrastructureStack(app, "InfrastructureStack");
            app.Synth();
        }
    }
}
