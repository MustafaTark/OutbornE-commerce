namespace OutbornE_commerce.FilesManager
{
    public interface IFilesManager
    {
        Task<FileModel?> UploadFile(IFormFile? file, string tagName);
        Task<List<FileModel>> UploadMultipleFile(List<IFormFile> lstFiles, string tagName);
        Task<bool> DeleteFile(string fileUrl);
        Task<bool> DeleteMultipleFiles(List<string> filesUrl);
    }
}
