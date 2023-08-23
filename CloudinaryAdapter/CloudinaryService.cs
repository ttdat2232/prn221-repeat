using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Interfaces.Adapters;

namespace CloudinaryAdapter
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadAsync(byte[] imageStream)
        {
            Stream uploadStream = new MemoryStream(imageStream);
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(Guid.NewGuid().ToString(), uploadStream),
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult.Url.ToString();
        }
    }
}
