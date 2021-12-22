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

        public string RecruiterId { get; set; }

        public Recruiter Recruiter { get; set; }

        public string CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        public string JobId { get; set; }

        public Job Job { get; set; }
    }
}
