using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Common.Entities.Notifications;
using System.Net.Http.Json;

namespace RabbitMq.IntegrationTests
{
    /*
    public class NotificationControllerTests : IntegrationTests
    {
        private static readonly string _privateNotificationUrl = "/api/privateNotification";

        private static readonly string _simpleNotificationUrl = "/api/simpleNotification";

        private static readonly SimpleNotificationDto _simpleNotificationModel = new()
        {
            Content = "Test",
            RecieverId = 1,
        };

        private static readonly PrivateNotificationDto _privateNotificationModel = new()
        {
            Content = "Test",
            SenderId = 1,
            RecieverId = 2,
        };

        public NotificationControllerTests()
        {
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<RabbitMqDb>();

                        var options = services.FirstOrDefault(descriptor =>
                            descriptor.ServiceType == typeof(DbContextOptions<RabbitMqDb>));

                        if (options is not null)
                            services.Remove(options);

                        services.AddDbContext<RabbitMqDb>(options =>
                        {
                            options.UseInMemoryDatabase("IntegrationTestsDb:Notification");
                        });
                    });
                });

            HttpClient = appFactory.CreateClient();
            _ = HttpClient.PostAsJsonAsync("/api/auth", RegisterModel).Result;
        }

        [Fact]
        public async Task GetNotifications_WhenNoNotifications_ThenReturnEmptyCollectionAndStatusCodeOk()
        {
            await Authenticate();

            var responseFromPrivateNotifications = await HttpClient.GetAsync(_privateNotificationUrl + "?userId=1");
            var responseFromSimpleNotifications = await HttpClient.GetAsync(_simpleNotificationUrl + "?userId=1");

            var privateNotifications = JsonConvert.DeserializeObject<List<PrivateNotification>>(
                await responseFromPrivateNotifications.Content.ReadAsStringAsync());

            var simpleNotifications = JsonConvert.DeserializeObject<List<PrivateNotification>>(
                await responseFromSimpleNotifications.Content.ReadAsStringAsync());

            privateNotifications.Should().BeEmpty();
            simpleNotifications.Should().BeEmpty();

            responseFromPrivateNotifications.StatusCode.Should().Be(HttpStatusCode.OK);
            responseFromSimpleNotifications.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetNotifications_WhenUserHasOneNotification_ThenReturnsCollectionWithOneElement()
        {
            await Authenticate();
            await HttpClient.PostAsJsonAsync(_simpleNotificationUrl, _simpleNotificationModel);

            var response = await HttpClient.GetAsync(_simpleNotificationUrl + "?userId=1");

            var notifications = JsonConvert.DeserializeObject<List<SimpleNotificationDto>>(
                await response.Content.ReadAsStringAsync());

            notifications.Should().ContainSingle();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateNotification_WhenNoRecieverId_ThenNotFoundStatusCode()
        {
            await Authenticate();

            var response = await HttpClient.PostAsJsonAsync(_simpleNotificationUrl, new SimpleNotificationDto()
            {
                Content = "Test",
            });

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateNotification_WhenModelValid_ThenSetCreatedAtAndSenderFields()
        {
            await Authenticate();
            await HttpClient.PostAsJsonAsync("/api/auth", new UserRegister()
            {
                Email = "Email",
                Password = "password",
                Username = "TestUser",
            });

            await HttpClient.PostAsJsonAsync(_privateNotificationUrl, _privateNotificationModel);

            var response = await HttpClient.GetAsync(_privateNotificationUrl + "?userId=2");

            var notifications = JsonConvert.DeserializeObject<List<PrivateNotificationDto>>(
                await response.Content.ReadAsStringAsync());

            Assert.NotNull(notifications);
            notifications.Should().ContainSingle();


            var sender = notifications.First().Sender;

            Assert.NotNull(sender);
            sender.Id.Should().Be(1);
            sender.Email.Should().Be("test");

            notifications[0].Content.Should().Be("Test");
            notifications[0].CreatedAt.ToLongDateString().Should().Be(DateTime.UtcNow.ToLongDateString());
        }
    }
    */
}
