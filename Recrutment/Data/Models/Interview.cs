using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recrutment.Data.Models
{
    using static DataConstants;

    public class Interview
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Date { get; set; }

        [Required]
        public string RecruiterId { get; set; }

        public Recruiter Recruiter { get; set; }

        [Required]
        public string CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        [Required]
        public string JobId { get; set; }

        public Job Job { get; set; }
    }
}
