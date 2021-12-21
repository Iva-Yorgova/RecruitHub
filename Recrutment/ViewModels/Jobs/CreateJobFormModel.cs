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

        public ICollection<JobSkill> Skills { get; set; } = new List<JobSkill>();
    }
}
