using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace OutbornE_commerce.FilesManager
{
    public class FilesManager : IFilesManager
    {
       private readonly IWebHostEnvironment _hostingEnvironment;
       private readonly IHttpContextAccessor _httpContextAccessor;
      public FilesManager(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
      {
         _hostingEnvironment = hostingEnvironment;
         _httpContextAccessor = httpContextAccessor;
      }
        public async Task<FileModel?> UploadFile(IFormFile? file, string tagName)
        {
            try
            {
                var configurationBuilder = new ConfigurationBuilder();
                var appSettingPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                configurationBuilder.AddJsonFile(appSettingPath, false);
                var root = configurationBuilder.Build();
                string baseUrl = root.GetSection("BaseUrl").Value;

                string schema = "https";
                var request = _httpContextAccessor.HttpContext!.Request;
                if (!request.IsHttps)
                {
                    schema = "http";
                }

                var defaultBaseUrl = $"{schema}://{request.Host}";
                string path = "Uploads/" + tagName + "/";
                string PhysicalfilePath = Path.Combine(_hostingEnvironment.WebRootPath, path);
                if (!Directory.Exists(PhysicalfilePath))
                {
                    Directory.CreateDirectory(PhysicalfilePath);
                }

                string fileName = Guid.NewGuid() + "." + file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                using (var stream = new FileStream(PhysicalfilePath + fileName, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                FileModel fileModel = new FileModel();
                fileModel.Url = defaultBaseUrl + "/" + path + fileName;
                fileModel.FileName = file.FileName;
                return fileModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<FileModel>> UploadMultipleFile(List<IFormFile> lstFiles, string tagName)
        {
            try
            {
                List<FileModel> fileModelList = new List<FileModel>();

                var configurationBuilder = new ConfigurationBuilder();
                var appSettingPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                configurationBuilder.AddJsonFile(appSettingPath, false);
                var root = configurationBuilder.Build();
                string baseUrl = root.GetSection("BaseUrl").Value;
                string path = "Uploads/" + tagName + "/";
                string PhysicalfilePath = Path.Combine(_hostingEnvironment.WebRootPath, path);

                if (!Directory.Exists(PhysicalfilePath))
                {
                    Directory.CreateDirectory(PhysicalfilePath);
                }
                foreach (IFormFile currentFile in lstFiles)
                {
                    FileModel fileModel = new FileModel();
                    string fileName = Guid.NewGuid() + "." + currentFile.FileName.Substring(currentFile.FileName.LastIndexOf(".") + 1);
                    using (var stream = new FileStream(PhysicalfilePath + fileName, FileMode.Create))
                    {
                        await currentFile.CopyToAsync(stream);
                    }

                    fileModel.Url = baseUrl + "/" + path + fileName;
                    fileModel.FileName = currentFile.FileName;
                    fileModelList.Add(fileModel);
                }
                return fileModelList;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<bool> DeleteFile(string fileUrl)
        {
            try
            {
                string decodedFileUrl = WebUtility.UrlDecode(fileUrl);

                if (!Uri.TryCreate(decodedFileUrl, UriKind.Absolute, out var fileUri))
                {
                    return false;
                }

                string localPath = fileUri.LocalPath;

                string webRootPath = _hostingEnvironment.WebRootPath;
                string fullPath = Path.Combine(webRootPath, localPath.TrimStart('/'));

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteMultipleFiles(List<string> filesUrl)
        {
            try
            {
                foreach (string fileUrl in filesUrl)
                {
                    string decodedFileUrl = WebUtility.UrlDecode(fileUrl);

                    // Ensure that the URL is valid
                    if (!Uri.TryCreate(decodedFileUrl, UriKind.Absolute, out var fileUri))
                    {
                        return false;
                    }

                    string localPath = fileUri.LocalPath;


                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string fullPath = Path.Combine(webRootPath, localPath.TrimStart('/'));

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);

                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
