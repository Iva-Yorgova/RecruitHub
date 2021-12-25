using Recrutment.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recrutment.ViewModels.Jobs
{
    public class CreateJobFormModel
    {
        public string Id { get; init; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Salary { get; set; }

        public string Skill { get; set; }

        public ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();

        public string InterviewDate { get; set; }

        public string RecruiterName { get; set; }

        public string CandidateName { get; set; }

        public string JobName { get; set; }
    }
}
