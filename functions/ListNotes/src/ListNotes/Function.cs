using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ListNotes
{
    public class Function
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            var tableName = System.Environment.GetEnvironmentVariable("TABLE_NAME");
            var notesTable = Table.LoadTable(client, tableName);
            var search = notesTable.Scan(new ScanFilter());
            var results = await search.GetNextSetAsync();

            return new APIGatewayProxyResponse
            {
                Body = results.ToJson(),
                Headers = new Dictionary<string, string> { {
                    "Content-Type", "application/json"
                } },
                StatusCode = 200,
            };
        }
    }
}
