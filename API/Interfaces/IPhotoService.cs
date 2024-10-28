using CloudinaryDotNet.Actions;

namespace API.Interfaces;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    Task<List<ImageUploadResult>> AddPhotosAsync(List<IFormFile> files);
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}
