namespace MobileRecharge.Application.HttpService;

public class HttpService : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

 
    public async Task<T> GetAsync<T>(string uri)
    {
        try
        {
            var _client = _httpClientFactory.CreateClient("Payment");

            string url = _client.BaseAddress + uri;

            _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage response = await _client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(responseBody);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return default(T);
        }
    }

    public async Task<TOut> PostAsync<TIn, TOut>(string uri, TIn content)
    {
        try
        {
            var _client = _httpClientFactory.CreateClient("Payment");

            _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

           
            string url = _client.BaseAddress + uri;

            var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await _client.PostAsync(url, serialized))
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TOut>(responseBody);
            }
        }
        catch (Exception ex)
        {
            return default(TOut);
        }
    }
}

public interface IHttpService
{
    Task<T> GetAsync<T>(string uri);

    Task<TOut> PostAsync<TIn, TOut>(string uri, TIn content);
}