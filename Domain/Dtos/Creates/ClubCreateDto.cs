using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.Creates
{
    public class ClubCreateDto
    {
        [Required]
        [MinLength(1), MaxLength(50)]
        public string Name { get; set; }
        public byte[]? Image { get; set; }
        [Display(Name = "President")]
        public long PresidentId { get; set; }
    }
}
