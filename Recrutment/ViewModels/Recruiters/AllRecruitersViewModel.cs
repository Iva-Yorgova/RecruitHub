using System.Collections.Generic;

namespace Recrutment.ViewModels.Recruiters
{
    public class AllRecruitersViewModel
    {
        public int Level { get; set; }
        public IEnumerable<RecruiterListingViewModel> Recruiters { get; set; }

    }
}
