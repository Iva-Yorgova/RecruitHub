

using System.Collections.Generic;

namespace Recrutment.ViewModels.Jobs
{
    public class AllJobsViewModel
    {
        public string SkillName { get; set; }
        public IEnumerable<JobListingViewModel> Jobs { get; set; }
    }
}
