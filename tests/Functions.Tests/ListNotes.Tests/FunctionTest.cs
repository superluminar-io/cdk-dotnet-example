using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Moq;
using Shared;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using Xunit;

namespace ListNotes.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async void TestListNotesFunction()
        {
            var tableName = "NOTES";
            System.Environment.SetEnvironmentVariable("TABLE_NAME", tableName);
            System.Environment.SetEnvironmentVariable("AWS_REGION", "eu-central-1");

            var attributesToGet = new List<string> { "id", "title", "content" };
            var item1 = new Dictionary<string, AttributeValue> {
                { "id", new AttributeValue { S = "abc"} },
                { "title", new AttributeValue { S = "hello world"} },
                { "content", new AttributeValue { S = "Et aliqua adipisicing incididunt eiusmod."} },
            };
            var items = new List<Dictionary<string, AttributeValue>> { item1 };
            var note = new Note
            {
                id = "abc",
                title = "hello world",
                content = "Et aliqua adipisicing incididunt eiusmod."
            };
            var notes = new List<Note> { note };

            var mock = new Mock<AmazonDynamoDBClient>();
            mock.Setup(library => library.ScanAsync(tableName, attributesToGet, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ScanResponse
                {
                    Items = items
                });

            var function = new Function(mock.Object);
            var response = await function.FunctionHandler();

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(notes), response.Body);
        }
    }
}