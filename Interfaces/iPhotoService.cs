using System;
using CloudinaryDotNet.Actions;

namespace webapp.Interfaces
{
	public interface iPhotoService
	{

		Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotoAsync(string publicId);



	}
}

