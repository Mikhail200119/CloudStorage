namespace CloudStorage.Web.Helpers;

public class CancellableFileStreamResult : IResult
{
    private const int BufferSize = 0x1000;
    
    private readonly CancellationToken _cancellationToken;

    public CancellableFileStreamResult(Stream fileStream, string contentType, CancellationToken cancellationToken) 
    {
        _cancellationToken = cancellationToken;
        ArgumentNullException.ThrowIfNull(fileStream);
        FileFileStream = fileStream;
    }

    public Stream FileFileStream { get; }
    
    public Task ExecuteAsync(HttpContext httpContext)
    {
        var outputStream = httpContext.Response.Body;

        using (FileFileStream)
        {
            var buffer = new byte[BufferSize];

            while (!_cancellationToken.IsCancellationRequested)
            {
                var bytesRead = FileFileStream.Read(buffer, 0, BufferSize);
                
                if (bytesRead == 0)
                {
                    break;
                }

                outputStream.Write(buffer, 0, bytesRead);
            }
        }

        return Task.CompletedTask;
    }
}