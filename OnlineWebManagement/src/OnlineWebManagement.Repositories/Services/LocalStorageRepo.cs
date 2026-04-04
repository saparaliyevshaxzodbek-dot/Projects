using System.IO.Compression;

namespace OnlineWebManagement.Repositories.Services;

public class LocalStorageRepo : IStorageRepositorys
{
    private readonly string BasePath;

    public LocalStorageRepo()
    {
        BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Data");
        if (!Directory.Exists(BasePath))
        {
            Directory.CreateDirectory(BasePath);
        }
    }

    public void CreateFolder(string folderPath)
    {
        var currentPath = Path.Combine(BasePath, folderPath);

        if (Directory.Exists(currentPath))
            throw new Exception($"Folder '{folderPath}' already exists");

        Directory.CreateDirectory(currentPath);
    }

    public void DeleteFile(string filePath)
    {
        var fullPath = Path.Combine(BasePath, filePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public void DeleteFolder(string folderPath)
    {
        var currentPath = Path.Combine(BasePath, folderPath);
        if (!Directory.Exists(currentPath))
            throw new Exception($"Folder '{folderPath}' does not exist");

        Directory.Delete(currentPath, true);
    }

    public async Task<Stream> DownloadFileAsync(string filePath)
    {
        var fullPath = Path.Combine(BasePath, filePath);
        if (!File.Exists(fullPath))
            throw new Exception($"File '{filePath}' does not exist");

        var memoryStream = new MemoryStream();
        using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        {
            await fileStream.CopyToAsync(memoryStream);
        }
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task<Stream> DownloadFolderAsZipAsync(string folderPath)
    {
        var fullPath = Path.Combine(BasePath, folderPath);
        if (!Directory.Exists(fullPath))
            throw new DirectoryNotFoundException($"Folder '{folderPath}' not found");

        var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            var files = Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(fullPath, file);
                var entry = archive.CreateEntry(relativePath);
                using (var entryStream = entry.Open())
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(entryStream);
                }
            }
        }
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task EditFileAsync(string filePath, string content)
    {
        var fullPath = Path.Combine(BasePath, filePath);
        if (!File.Exists(fullPath))
            throw new Exception($"File '{filePath}' does not exist");

        using (var writer = new StreamWriter(fullPath, false))
        {
            await writer.WriteAsync(content);
        }
    }

    public List<string> GetAll(string folderPath)
    {
        var fullPath = Path.Combine(BasePath, folderPath);
        if (!Directory.Exists(fullPath))
            return new List<string>();

        var entries = new List<string>();

        // Papkalarni qo'shish (faqat nomi)
        foreach (var dir in Directory.GetDirectories(fullPath))
        {
            entries.Add(Path.GetFileName(dir));
        }

        // Fayllarni qo'shish (faqat nomi)
        foreach (var file in Directory.GetFiles(fullPath))
        {
            entries.Add(Path.GetFileName(file));
        }

        return entries;
    }

    public async Task<List<string>> GetTextOfFileAsync(string filePath)
    {
        var fullPath = Path.Combine(BasePath, filePath);
        if (!File.Exists(fullPath))
            throw new Exception($"File '{filePath}' does not exist");

        var lines = await File.ReadAllLinesAsync(fullPath);
        return lines.ToList();
    }

    public async Task UploadFileAsync(string filePath, Stream stream)
    {
        var fullPath = Path.Combine(BasePath, filePath);
        var directory = Path.GetDirectoryName(fullPath);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            await stream.CopyToAsync(fileStream);
        }
    }
}