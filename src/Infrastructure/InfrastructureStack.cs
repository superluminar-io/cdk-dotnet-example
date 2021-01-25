using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.APIGatewayv2;
using Amazon.CDK.AWS.APIGatewayv2.Integrations;
using Amazon.CDK.AWS.DynamoDB;

namespace Infrastructure
{
    public class InfrastructureStack : Stack
    {
        public InfrastructureStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var table = new Table(this, "NotesTable", new TableProps
            {
                PartitionKey = new Attribute
                {
                    Name = "id",
                    Type = AttributeType.STRING
                },
                RemovalPolicy = RemovalPolicy.DESTROY,
                BillingMode = BillingMode.PAY_PER_REQUEST,
            });

            var api = new HttpApi(this, "NotesApi");

            // List notes
            var functionListNotes = new DockerImageFunction(this, "FunctionListNotes", new DockerImageFunctionProps
            {
                Code = DockerImageCode.FromImageAsset("src/Functions/ListNotes"),
                Timeout = Duration.Seconds(10),
                MemorySize = 1024,
                Environment = new Dictionary<string, string>
                {
                    { "TABLE_NAME", table.TableName }
                }
            });

            table.GrantFullAccess(functionListNotes);

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
            var functionGetNote = new DockerImageFunction(this, "FunctionGetNote", new DockerImageFunctionProps
            {
                Code = DockerImageCode.FromImageAsset("src/Functions/GetNote"),
                Timeout = Duration.Seconds(10),
                MemorySize = 1024,
                Environment = new Dictionary<string, string>
                {
                    { "TABLE_NAME", table.TableName }
                }
            });

            table.GrantFullAccess(functionGetNote);

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

            // Create note
            var functionCreateNote = new DockerImageFunction(this, "FunctionCreateNote", new DockerImageFunctionProps
            {
                Code = DockerImageCode.FromImageAsset("src/Functions/CreateNote"),
                Timeout = Duration.Seconds(10),
                MemorySize = 1024,
                Environment = new Dictionary<string, string>
                {
                    { "TABLE_NAME", table.TableName }
                }
            });

            table.GrantFullAccess(functionCreateNote);

            var integrationCreateNote = new LambdaProxyIntegration(new LambdaProxyIntegrationProps
            {
                Handler = functionCreateNote,
            });

            HttpMethod[] methodsCreateNote = { HttpMethod.POST };
            api.AddRoutes(new AddRoutesOptions
            {
                Path = "/notes",
                Integration = integrationCreateNote,
                Methods = methodsCreateNote,
            });

            new CfnOutput(this, "NotesApiUrl", new CfnOutputProps
            {
                Value = api.ApiEndpoint
            });
        }
    }
}
