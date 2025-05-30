﻿using Microsoft.AspNetCore.Http;

namespace TaskManagment.Core.Service
{
    public interface IFileService
    {
        Task<(bool isUploaded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath, bool hasThumbnail);
        void Delete(string imagePath, string? imageThumbnailPath = null);
    }
}