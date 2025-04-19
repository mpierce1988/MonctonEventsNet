using Microsoft.Extensions.Configuration;

namespace MonctonEventsNet.Application.FileProvider;

public class LocalFileProvider : IFileProvider
{
    #region Private Fields

    private string? _filePath;
    
    #endregion
    
    #region Constructor

    public LocalFileProvider(IConfiguration configuration)
    {
        _filePath = configuration["LocalFileProvider:FilePath"];
    }
    
    #endregion
    
    public async Task<Stream> GetEventsExcelFileAsync()
    {
        if (_filePath is null) throw new ArgumentNullException(nameof(_filePath), "File path is null");

        byte[] fileStream = await File.ReadAllBytesAsync(_filePath);

        return new MemoryStream(fileStream);
    }
}