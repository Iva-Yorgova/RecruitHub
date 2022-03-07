using MyWebServer.Controllers;
using MyWebServer.Http;
using Recrutment.Data;
using Recrutment.ViewModels.Interviews;
using System.Linq;

namespace Recrutment.Controllers
{
    public class InterviewsController : Controller
    {
        private readonly RecrutmentDbContext data;

        public InterviewsController(RecrutmentDbContext data)
            => this.data = data;

        public HttpResponse All()
        {
            var interviews = this.data
                .Interviews
                .OrderBy(i => i.Date)
                .Select(i => new InterviewListingViewModel
                {
                    Id = i.Id,
                    Date = i.Date,
                    CandidateName = i.CandidateName,
                    RecruiterName = i.RecruiterName,
                    JobName = i.JobName
                })
                .ToList();

            return View(interviews);
        }

        public HttpResponse Delete(string id)
        {
            var interview = this.data
                .Interviews
                .Where(i => i.Id == id)
                .FirstOrDefault();

            var recruiter = this.data.Recruiters
                        .Where(r => r.Name == interview.RecruiterName)
                        .FirstOrDefault();

            recruiter.FreeInterviewSlots++;

            this.data.Interviews.Remove(interview);
            this.data.SaveChanges();

            return Redirect("/Interviews/All");
        }

        public HttpResponse Details(string id)
        {
            var interview = this.data
                .Interviews
                .Where(i => i.Id == id)
                .Select(i => new InterviewListingViewModel 
                {
                    Id = i.Id,
                    Date = i.Date,
                    CandidateName = i.CandidateName,
                    RecruiterName = i.RecruiterName,
                    JobName = i.JobName
                })
                .FirstOrDefault();

            return View(interview);
        }
    }
}
