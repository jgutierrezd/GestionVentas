using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebGestionVentas.Pages.Gerente
{
    public class ReporteVentaModel : PageModel
    {
        public string MyProperty { get; set; }

        private const string DefaultMediaType = "application/json";
        private readonly HttpClient _httpClient;

        public ReporteVentaModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("quotes.api");
        }

        public async Task<string> GetToken()
        {
            var content = new StringContent("{\"UserName\":\"Admin\",\r\n\"Password\":\"123456\"}", null, DefaultMediaType);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44394/token");
            request.Content = content;

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetSales(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpResponseMessage response = await _httpClient.GetAsync("products");
            
            if (response.IsSuccessStatusCode)
                response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async void OnGet()
        {
            var resultToken = await GetToken();
            MyProperty = await GetSales(resultToken);
        }
    }
}
