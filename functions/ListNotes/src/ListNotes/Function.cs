using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

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
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            var note1 = new Note();
            note1.id = "abc";
            note1.title = "Hello World";
            note1.content = "Culpa qui veniam sint minim ut aliquip officia veniam voluptate.";

            var note2 = new Note();
            note2.id = "abc";
            note2.title = "Hello World";
            note2.content = "Culpa qui veniam sint minim ut aliquip officia veniam voluptate.";

            Note[] notes = { note1, note2 };

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
