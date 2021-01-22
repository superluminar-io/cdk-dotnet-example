using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.APIGatewayv2;
using Amazon.CDK.AWS.APIGatewayv2.Integrations;

namespace LambdaDotnetExample
{
    public class LambdaDotnetExampleStack : Stack
    {
        public LambdaDotnetExampleStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var api = new HttpApi(this, "NotesApi");

            // List notes
            var functionListNotes = new Function(this, "FunctionListNotes", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("./functions/ListNotes/src/ListNotes/bin/Release/netcoreapp3.1/publish"),
                Handler = "ListNotes::ListNotes.Function::FunctionHandler"
            });

            var integrationListNotes = new LambdaProxyIntegration(new LambdaProxyIntegrationProps
            {
                Handler = functionListNotes,
            });

            HttpMethod[] methodsListNotes = { HttpMethod.GET };
            api.AddRoutes(new AddRoutesOptions
            {
                Path = "/notes",
                Integration = integrationListNotes,
                Methods = methodsListNotes,
            });

            // Get note
            var functionGetNote = new Function(this, "FunctionGetNote", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("./functions/GetNote/src/GetNote/bin/Release/netcoreapp3.1/publish"),
                Handler = "GetNote::GetNote.Function::FunctionHandler"
            });

            var integrationGetNote = new LambdaProxyIntegration(new LambdaProxyIntegrationProps
            {
                Handler = functionGetNote,
            });

            HttpMethod[] methodsGetNote = { HttpMethod.GET };
            api.AddRoutes(new AddRoutesOptions
            {
                Path = "/notes/{id}",
                Integration = integrationGetNote,
                Methods = methodsGetNote,
            });

            new CfnOutput(this, "NotesApiUrl", new CfnOutputProps
            {
                Value = api.ApiEndpoint
            });
        }
    }
}
