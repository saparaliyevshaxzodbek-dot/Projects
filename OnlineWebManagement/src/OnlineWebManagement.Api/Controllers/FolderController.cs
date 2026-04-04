using Microsoft.AspNetCore.Mvc;
using OnlineWebManagement.Services.Services;

namespace OnlineWebManagement.Api.Controllers;

[Route("api/folders")]
[ApiController]
public class FolderController : ControllerBase
{
    private readonly IStorageService StorageService;

    public FolderController()
    {
        StorageService = new LocalStorageService();
    }

    // 1. Papka yaratish (sizda bor)
    [HttpPost("create-folder")]
    public IActionResult AddFolder(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
            return BadRequest("folderPath is required");

        StorageService.CreateFolder(folderPath);
        return Ok($"Folder '{folderPath}' created successfully");
    }

    // 2. Papkani o'chirish (sizda bor)
    [HttpDelete("delete-folder")]
    public IActionResult DeleteFolder(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
            return BadRequest("folderPath is required");

        StorageService.DeleteFolder(folderPath);
        return Ok($"Folder '{folderPath}' deleted successfully");
    }

    // 3. Papka ichidagi barcha fayl va papkalarni olish
    [HttpGet("get-all")]
    public IActionResult GetAll(string folderPath = "")
    {
        var result = StorageService.GetAll(folderPath ?? "");
        return Ok(result);
    }

    // 4. Faylni yuklab olish
    [HttpGet("download-file")]
    public async Task<IActionResult> DownloadFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return BadRequest("filePath is required");

        var stream = await StorageService.DownloadFileAsync(filePath);
        var fileName = Path.GetFileName(filePath);
        return File(stream, "application/octet-stream", fileName);
    }

    // 5. Matn faylini o'qish
    [HttpGet("file-text")]
    public async Task<IActionResult> GetFileText(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return BadRequest("filePath is required");

        var lines = await StorageService.GetTextOfFileAsync(filePath);
        return Ok(lines);
    }

    // 6. Butun papkani ZIP qilib yuklab olish
    [HttpGet("download-folder")]
    public async Task<IActionResult> DownloadFolder(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
            return BadRequest("folderPath is required");

        var stream = await StorageService.DownloadFolderAsZipAsync(folderPath);
        var folderName = Path.GetFileName(folderPath);
        return File(stream, "application/zip", $"{folderName}.zip");
    }

    // 7. Fayl yuklash (upload)
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(string filePath, IFormFile file)
    {
        if (string.IsNullOrEmpty(filePath))
            return BadRequest("filePath is required");

        if (file == null || file.Length == 0)
            return BadRequest("File is required");

        using (var stream = file.OpenReadStream())
        {
            await StorageService.UploadFileAsync(filePath, stream);
        }

        return Ok($"File '{filePath}' uploaded successfully");
    }

    // 8. Faylni tahrirlash
    [HttpPut("edit-file")]
    public async Task<IActionResult> EditFile(string filePath, [FromBody] string content)
    {
        if (string.IsNullOrEmpty(filePath))
            return BadRequest("filePath is required");

        if (content == null)
            return BadRequest("Content is required");

        await StorageService.EditFileAsync(filePath, content);
        return Ok($"File '{filePath}' edited successfully");
    }

    // 9. Faylni o'chirish
    [HttpDelete("delete-file")]
    public IActionResult DeleteFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return BadRequest("filePath is required");

        StorageService.DeleteFile(filePath);
        return Ok($"File '{filePath}' deleted successfully");
    }
}