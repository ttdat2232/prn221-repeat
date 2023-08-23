namespace Domain.Interfaces.Adapters
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(byte[] imageStream);
    }
}
