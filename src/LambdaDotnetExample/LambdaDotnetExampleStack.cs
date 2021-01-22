using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.APIGateway;

namespace LambdaDotnetExample
{
    public class LambdaDotnetExampleStack : Stack
    {
        public LambdaDotnetExampleStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var hello = new Function(this, "HelloHandler", new FunctionProps
            {
                Runtime = Runtime.NODEJS_12_X,
                Code = Code.FromAsset("lambda"),
                Handler = "hello.handler"
            });

            new LambdaRestApi(this, "Endpoint", new LambdaRestApiProps
            {
                Handler = hello
            });
        }
    }
}
