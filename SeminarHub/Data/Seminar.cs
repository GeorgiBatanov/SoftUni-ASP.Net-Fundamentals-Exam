using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeminarHub.Data
{
    public class Seminar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(DataRequirments.TopicMinLength)]
        [MaxLength(DataRequirments.TopicMaxLength)]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [MinLength(DataRequirments.LecturerNameMinLength)]
        [MaxLength(DataRequirments.LecturerNameMaxLength)]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [MinLength(DataRequirments.DetailsMinLength)]
        [MaxLength(DataRequirments.DetailsMaxLength)]
        public string Details { get; set; } = string.Empty;

        [Required]
        public string OrganizerId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(OrganizerId))]
        public IdentityUser Organizer { get; set; } = null!;

        [Required]
        public DateTime DateAndTime { get; set; }

        [Range(DataRequirments.DurationMin, DataRequirments.DurationMax)]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public IList<SeminarParticipant> SeminarParticipants { get; set; } = new List<SeminarParticipant>();
    }
}
