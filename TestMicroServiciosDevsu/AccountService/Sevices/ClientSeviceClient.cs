namespace AccountService.Sevices
{
    public record ClientDto(Guid Id, string Name, string CI, string Addres, bool State);
    public interface IClientServiceClient
    {
        Task<ClientDto?> GetClientAsync(Guid clientId);
    }
    public class ClientSeviceClient : IClientServiceClient
    {
        private readonly HttpClient _httpClient;

        public ClientSeviceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ClientDto?> GetClientAsync(Guid clientId)
        {
            var response = await _httpClient.GetAsync($"/api/clientes/{clientId}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ClientDto>();
        }
    }
}
