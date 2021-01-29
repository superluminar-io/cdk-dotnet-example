using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Moq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using Xunit;

namespace GetNote.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async void TestGetNoteFunction()
        {
            var tableName = "NOTES";
            System.Environment.SetEnvironmentVariable("TABLE_NAME", tableName);

            var id = "abc";
            var title = "hello world";
            var content = "Et aliqua adipisicing incididunt eiusmod.";

            var item = new Dictionary<string, AttributeValue> {
                { "id", new AttributeValue { S = id} },
                { "title", new AttributeValue { S = title } },
                { "content", new AttributeValue { S = content } },
            };

            var note = new Note
            {
                id = id,
                title = title,
                content = content
            };

            var mock = new Mock<AmazonDynamoDBClient>();
            mock.Setup(library => library.GetItemAsync(It.IsAny<GetItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetItemResponse
                {
                    Item = item
                });

            var getNote = new Function(mock.Object);
            var response = await getNote.FunctionHandler(new APIGatewayProxyRequest
            {
                PathParameters = new Dictionary<string, string> {
                    { "id", id }
                }
            });

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(note), response.Body);
        }
    }
}