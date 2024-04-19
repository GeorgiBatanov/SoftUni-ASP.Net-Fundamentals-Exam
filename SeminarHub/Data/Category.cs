using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeminarHub.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(DataRequirments.CetegoryNameMinLength)]
        [MaxLength(DataRequirments.CetegoryNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Seminar> Seminars { get; set; } = new List<Seminar>();
    }
}
