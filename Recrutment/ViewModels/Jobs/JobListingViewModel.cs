
using Recrutment.Data.Models;
using System.Collections.Generic;

namespace Recrutment.ViewModels.Jobs
{
    public class JobListingViewModel
    {
        public string Id { get; init; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Salary { get; set; }

        public ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();

        public int Interviews { get; set; }

        public ICollection<Skill> AllSkills { get; set; } = new List<Skill>();
    }
}
