using Domain.Dtos.Base;

namespace Domain.Dtos.Updates
{
    public class ClubUpdateDto : BaseDto
    {
        public string? Name { get; set; }
        public byte[]? Image { get; set; }
    }
}
