using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TaskManagment.Core.Abstractions.Const;
using TaskManagment.Core.Service;
using TaskManagment.Core.UnitOfWork;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace TaskManagment.ServiceAndFactore.Service
{
    public class ImageService(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork) : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 2 * 1024 * 1024; // 5 MB

        public async Task<(bool isUploaded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath, bool hasThumbnail)
        {
            var extension = Path.GetExtension(image.FileName);

            if (!_allowedExtensions.Contains(extension))
                return (isUploaded: false, errorMessage: Errors.AllowedExtension);

            if (image.Length > MaxFileSize)
                return (isUploaded: false, errorMessage: Errors.MaxImageSaize);

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}{folderPath}", imageName);

            using var stream = File.Create(path);
            await image.CopyToAsync(stream);
            stream.Dispose();

            if (hasThumbnail)
            {
                var thumbPath = Path.Combine($"{_webHostEnvironment.WebRootPath}{folderPath}/thumb", imageName);

                using var loadedImage = Image.Load(image.OpenReadStream());
                var ratio = (float)loadedImage.Width / 200;
                var height = loadedImage.Height / ratio;
                loadedImage.Mutate(i => i.Resize(width: 200, height: (int)height));
                loadedImage.Save(thumbPath);
            }

            return (isUploaded: true, errorMessage: null);
        }

        public void Delete(string imagePath, string? imageThumbnailPath = null)
        {
            var oldImagePath = $"{_webHostEnvironment.WebRootPath}{imagePath}";

            if (File.Exists(oldImagePath))
                File.Delete(oldImagePath);

            if (!string.IsNullOrEmpty(imageThumbnailPath))
            {
                var oldThumbPath = $"{_webHostEnvironment.WebRootPath}{imageThumbnailPath}";

                if (File.Exists(oldThumbPath))
                    File.Delete(oldThumbPath);
            }
        }

    }

}
