using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ListNotes
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


        public async Task<APIGatewayProxyResponse> FunctionHandler()
        {
            var tableName = System.Environment.GetEnvironmentVariable("TABLE_NAME");
            var response = await client.ScanAsync(tableName, new List<string> { "id", "title", "content" });
            var notes = response.Items.ConvertAll<Note>((values) => new Note
            {
                id = values["id"].S,
                title = values["title"].S,
                content = values["content"].S
            });

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(notes),
                Headers = new Dictionary<string, string> { {
                    "Content-Type", "application/json"
                } },
                StatusCode = 200,
            };
        }
    }
}
