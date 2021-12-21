

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recrutment.Data.Models
{
    using static DataConstants;

    public class Job
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Salary { get; set; }

        public ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();

       

    }
}
