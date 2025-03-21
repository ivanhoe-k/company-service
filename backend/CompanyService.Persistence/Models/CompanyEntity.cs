using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyService.Persistence.Models
{
    public sealed class CompanyEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string ExchangeMicCode { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Ticker { get; set; } = null!;

        [Required]
        [MaxLength(12)]
        public string Isin { get; set; } = null!;

        [MaxLength(2000)] // Standard max length for URLs
        public Uri? Website { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Cursor { get; set; }
    }
}
