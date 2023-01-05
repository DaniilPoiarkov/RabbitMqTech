namespace RabbitMq.IntegrationTests
{
    public class TestControllerTests : IntegrationTests
    {
        [Fact]
        public async Task Test_WhenCalls_ReturnsTestString()
        {
            var response = await HttpClient.GetAsync("/api/test");

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Be("Test");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
