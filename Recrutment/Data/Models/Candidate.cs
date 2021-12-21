
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Recrutment.Data.Models
{
    using static DataConstants;

    public class Candidate
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Bio { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public string RecruiterId { get; set; }

        public Recruiter Recruiter { get; set; }

        public ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();

    }
}
