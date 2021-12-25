

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recrutment.Data.Models
{
    using static DataConstants;

    public class Recruiter
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public string Epost { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public int ExperienceLevel { get; set; } = 1;

        public int FreeInterviewSlots { get; set; } = 5;

        public ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
    }
}
