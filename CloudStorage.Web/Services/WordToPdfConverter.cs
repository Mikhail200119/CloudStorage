using CloudStorage.Web.Services.Interfaces;

namespace CloudStorage.Web.Services;

public class WordToPdfConverter : IWordToPdfConverter
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WordToPdfConverter(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Stream> GetConvertedFile(Stream data)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(WordToPdfConverter));
        await Authenticate();
        
        return Stream.Null;
    }

    private async Task Authenticate()
    {
        var client = _httpClientFactory.CreateClient(nameof(WordToPdfConverter));

        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(@"https://api.ilovepdf.com/v1/auth", UriKind.Absolute)
        };

        var authenticateResponse = await client.SendAsync(requestMessage);
        var token = await authenticateResponse.Content.ReadAsStringAsync();
    }
}