using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetNote
{
    public class Note
    {
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }

    public class Function
    {
        private AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public Function() { }

        public Function(AmazonDynamoDBClient c) => client = c;

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent)
        {
            var id = apigProxyEvent.PathParameters["id"];
            var tableName = System.Environment.GetEnvironmentVariable("TABLE_NAME");

            var result = await client.GetItemAsync(new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "id", new AttributeValue {
                        S = id
                    } }
                },
                ProjectionExpression = "id, title, content",
                ConsistentRead = true
            });

            if (result.Item.Count == 0)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 400,
                };
            }

            var note = new Note
            {
                id = result.Item["id"].S,
                title = result.Item["title"].S,
                content = result.Item["content"].S
            };

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(note),
                Headers = new Dictionary<string, string> { {
                    "Content-Type", "application/json"
                } },
                StatusCode = 200,
            };
        }
    }
}
