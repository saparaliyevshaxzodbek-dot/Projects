# OnlineWebManagement — File Storage API

A layered ASP.NET Core Web API for working with folders and files, where the storage is hidden behind an interface.

## What's inside

- `OnlineWebManagement.Api` — `FolderController`
- `OnlineWebManagement.Services` — `IStorageService` / `LocalStorageService`
- `OnlineWebManagement.Repositories` — `IStorageRepository` with two implementations: `LocalStorageRepo` (disk) and `AwsStorageRepo` (AWS S3)

## Tech

C# · ASP.NET Core Web API · AWS S3 · Swagger

## Run

Requires the .NET SDK.

```bash
cd OnlineWebManagement/src/OnlineWebManagement.Api
dotnet run
```

---

Part of my C# / .NET training at **PDP Academy** (2025–2026). My main projects: [Dasturxon](https://github.com/saparaliyevshaxzodbek-dot/dasturxon) · [Portfolio](https://github.com/saparaliyevshaxzodbek-dot/portfolio) · [profile](https://github.com/saparaliyevshaxzodbek-dot).
