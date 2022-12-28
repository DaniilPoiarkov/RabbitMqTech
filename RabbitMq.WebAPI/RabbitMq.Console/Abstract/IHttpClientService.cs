using RabbitMq.Common.DTOs;

namespace RabbitMq.Console.Abstract
{
    public interface IHttpClientService
    {
        HttpClient HttpClient { get; }
        Task<string> Login(string email, string password);
        Task<UserDto> GetCurrentUser();

        StringContent CreateRequestStringContent(object body);
        Task<HttpResponseMessage> GetRequest(string url);
        Task<HttpResponseMessage> PostRequest(string url, StringContent? body = null);
        Task<HttpResponseMessage> PutRequest(string url, StringContent? body = null);
        Task<HttpResponseMessage> DeleteRequest(string url);
    }
}
