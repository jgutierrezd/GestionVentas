using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;

namespace WebGestionVentas.Pages.Gerente
{
    public class ReporteVentaModel : PageModel
    {
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
            //var result1 = await _httpClient.SendAsync(request);

            //var result2 = await _httpClient.PostAsync("token", content);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async void OnGet()
        {
            var result = await GetToken();
        }
    }
}
