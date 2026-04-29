using FluentFTP;

namespace FLM_LobbyDisplay.Services;

public class FileServerTransferService
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<FileServerTransferService> _logger;

    public FileServerTransferService(IConfiguration config, IWebHostEnvironment env, ILogger<FileServerTransferService> logger)
    {
        _config = config;
        _env = env;
        _logger = logger;
    }

    public async Task<bool> SaveAsync(IFormFile file, string destDirectory, string destFileName)
    {
        try
        {
            var destPath = Path.Combine(_env.WebRootPath, destDirectory);
            Directory.CreateDirectory(destPath);
            var fullPath = Path.Combine(destPath, destFileName);
            if (File.Exists(fullPath)) File.Delete(fullPath);
            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "File save failed: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<bool> TransferFileAsync(string filename, byte[] content)
    {
        try
        {
            var host = _config["AppSettings:FILESERVER_KEY"] ?? string.Empty;
            var path = _config["AppSettings:FILESERVER_PATH"] ?? string.Empty;
            // FluentFTP upload — credentials would come from config in production
            using var ftp = new AsyncFtpClient(host);
            await ftp.Connect();
            await ftp.UploadBytes(content, $"{path}/{filename}");
            await ftp.Disconnect();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FTP transfer failed: {Message}", ex.Message);
            return false;
        }
    }
}
