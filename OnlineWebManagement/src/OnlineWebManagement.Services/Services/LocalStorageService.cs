using OnlineWebManagement.Repositories.Services;

namespace OnlineWebManagement.Services.Services;

public class LocalStorageService : IStorageService
{
    private readonly IStorageRepositorys StorageRepositorys;

    public LocalStorageService()
    {
        StorageRepositorys = new LocalStorageRepo();
    }
    public void CreateFolder(string folderPath)
    {
        StorageRepositorys.CreateFolder(folderPath);
    }
    public void DeleteFolder(string folderPath)
    {
        StorageRepositorys.DeleteFolder(folderPath);
    }

    public async Task UploadFileAsync(string filePath, Stream stream)
    {
        // Service faqat repository ni chaqiradi
        await StorageRepositorys.UploadFileAsync(filePath, stream);
    }

    public async Task<Stream> DownloadFileAsync(string filePath)
    {
        // Service faqat repository ni chaqiradi
        return await StorageRepositorys.DownloadFileAsync(filePath);
    }

    public async Task EditFileAsync(string filePath, string content)
    {
        // Qo‘shimcha tekshiruvlar (biznes mantiq) bo‘lishi mumkin
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty");

        await StorageRepositorys.EditFileAsync(filePath, content);
    }

    public List<string> GetAll(string folderPath)
    {
        return StorageRepositorys.GetAll(folderPath);
    }

    // 2. Matn faylining barcha qatorlarini o‘qish (async)
    public async Task<List<string>> GetTextOfFileAsync(string filePath)
    {
        return await StorageRepositorys.GetTextOfFileAsync(filePath);
    }

    // 3. Butun papkani ZIP qilib yuklab olish (async)
    public async Task<Stream> DownloadFolderAsZipAsync(string folderPath)
    {
        return await StorageRepositorys.DownloadFolderAsZipAsync(folderPath);
    }

    // 4. Faylni o‘chirish
    public void DeleteFile(string filePath)
    {
        StorageRepositorys.DeleteFile(filePath);
    }

}
