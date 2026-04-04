namespace OnlineWebManagement.Repositories.Services;

public interface IStorageRepositorys
{
    public void CreateFolder(string folderPath);
    public void DeleteFolder(string folderPath);
    public Task UploadFileAsync(string filePath, Stream stream);
    public void DeleteFile(string filePath);
    public Task<Stream> DownloadFileAsync(string filePath);
    public Task<Stream> DownloadFolderAsZipAsync(string filePath);
    public List<string> GetAll(string folderPath);
    public Task<List<string>> GetTextOfFileAsync(string filePath);
    public Task EditFileAsync(string filePath, string content);
}
