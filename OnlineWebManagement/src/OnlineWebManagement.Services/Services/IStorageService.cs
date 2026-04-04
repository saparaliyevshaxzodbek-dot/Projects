namespace OnlineWebManagement.Services.Services;

public interface IStorageService
{
    Task UploadFileAsync(string filePath, Stream stream);
    Task<Stream> DownloadFileAsync(string filePath);
    Task EditFileAsync(string filePath, string content);
    List<string> GetAll(string folderPath);
    Task<List<string>> GetTextOfFileAsync(string filePath);
    Task<Stream> DownloadFolderAsZipAsync(string folderPath);
    void DeleteFile(string filePath);
    void CreateFolder(string folderPath);
    void DeleteFolder(string folderPath);
}