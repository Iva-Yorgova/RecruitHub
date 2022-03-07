using Recrutment.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recrutment.ViewModels.Interviews
{
    public class InterviewListingViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Date { get; set; }

        public string RecruiterName { get; set; }

        public string CandidateName { get; set; }

        public string JobName { get; set; }

    }
}
