namespace MonctonEventsNet.Application.FileProvider;

public interface IFileProvider
{
    public Task<Stream> GetEventsExcelFileAsync();
}