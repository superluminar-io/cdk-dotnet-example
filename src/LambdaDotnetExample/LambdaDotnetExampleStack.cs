using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.APIGateway;

namespace LambdaDotnetExample
{
    public class LambdaDotnetExampleStack : Stack
    {
        public LambdaDotnetExampleStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var function = new Function(this, "DotNet", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("./functions/LambdaDotnetExample/src/LambdaDotnetExample/bin/Release/netcoreapp3.1/publish"),
                Handler = "LambdaDotnetExample::LambdaDotnetExample.Function::FunctionHandler"
            });

            new LambdaRestApi(this, "Endpoint", new LambdaRestApiProps
            {
                Handler = function
            });
        }
    }
}
