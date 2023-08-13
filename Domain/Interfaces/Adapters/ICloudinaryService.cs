using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Adapters
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(byte[] imageStream);
    }
}
