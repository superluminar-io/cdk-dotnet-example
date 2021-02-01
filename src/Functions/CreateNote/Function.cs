using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CreateNote
{
    public class RequestBody
    {
        public string title { get; set; }
        public string content { get; set; }
    }

    public class Function
    {
        private AmazonDynamoDBClient client;

        public Function() => client = new AmazonDynamoDBClient();

        public Function(AmazonDynamoDBClient c) => client = c;

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent)
        {
            var requestBody = JsonSerializer.Deserialize<RequestBody>(apigProxyEvent.Body);

            var note = new Note
            {
                id = Guid.NewGuid().ToString(),
                title = requestBody.title,
                content = requestBody.content,
            };

            var tableName = System.Environment.GetEnvironmentVariable("TABLE_NAME");
            var result = await client.PutItemAsync(new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string, AttributeValue>()
                    {
                        { "id", new AttributeValue { S = note.id }},
                        { "title", new AttributeValue { S = note.title }},
                        { "content", new AttributeValue { S = note.content }}
                    }
            });

            if (result.HttpStatusCode != HttpStatusCode.OK)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)result.HttpStatusCode,
                };
            }

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(note),
                Headers = new Dictionary<string, string> { {
                    "Content-Type", "application/json"
                } },
                StatusCode = (int)HttpStatusCode.Created,
            };
        }
    }
}
