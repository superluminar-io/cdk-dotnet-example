using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Moq;
using Shared;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading;
using Xunit;

namespace CreateNote.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async void TestCreateNoteFunction()
        {
            var tableName = "NOTES";
            System.Environment.SetEnvironmentVariable("TABLE_NAME", tableName);
            System.Environment.SetEnvironmentVariable("AWS_REGION", "eu-central-1");

            var requestBody = new RequestBody
            {
                title = "hello world",
                content = "Et aliqua adipisicing incididunt eiusmod.",
            };

            var mock = new Mock<AmazonDynamoDBClient>();
            mock.Setup(library => library.PutItemAsync(It.IsAny<PutItemRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new PutItemResponse
                    {
                        HttpStatusCode = HttpStatusCode.OK
                    });

            var getNote = new Function(mock.Object);
            var response = await getNote.FunctionHandler(new APIGatewayProxyRequest
            {
                Body = JsonSerializer.Serialize(requestBody),
            });

            var note = JsonSerializer.Deserialize<Note>(response.Body);

            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(requestBody.title, note.title);
            Assert.Equal(requestBody.content, note.content);
        }
    }
}