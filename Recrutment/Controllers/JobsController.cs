
using MyWebServer.Controllers;
using MyWebServer.Http;
using Recrutment.Data;
using Recrutment.Data.Models;
using Recrutment.ViewModels.Jobs;
using System.Linq;

namespace Recrutment.Controllers
{
    public class JobsController : Controller
    {
        private readonly RecrutmentDbContext data;

        public JobsController(RecrutmentDbContext data)
            => this.data = data;

        public HttpResponse All()
        {
            var jobs = this.data
                .Jobs
                .OrderBy(j => j.Title)
                .Select(j => new JobListingViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Salary = j.Salary,
                    JobSkills = j.JobSkills
                })
                .ToList();

            return View(jobs);
        }

        public HttpResponse Create()
        {
            return View();
        }

        [HttpPost]
        public HttpResponse Create(CreateJobFormModel model)
        {
            // Validation!

            var job = new Job
            {
                Title = model.Title,
                Description = model.Description,
                Salary = model.Salary  
            }; 

            if (model.Skill != null)
            {
                if (!this.data.Skills.Any(s => s.Name == model.Skill))
                {
                    var skill = new JobSkill { Name = model.Skill };
                    job.JobSkills.Add(skill);
                    this.data.Skills.Add(new Skill { Name = model.Skill });
                }
                else
                {
                    job.JobSkills.Add(new JobSkill { Name = model.Skill });
                }
            }

            var jobSkills = job.JobSkills.Select(j => j.Name).ToList();

            var candidates = this.data.Candidates
                .Where(c => c.CandidateSkills.Any(s => jobSkills.Contains(s.Name))).ToList();

            if (candidates.Count > 0)
            {
                foreach (var candidate in candidates)
                {
                    var interview = new Interview
                    {
                        Date = model.InterviewDate,
                        CandidateId = candidate.Id,
                        Candidate = candidate,
                        RecruiterId = candidate.RecruiterId,
                        Recruiter = candidate.Recruiter,
                        Job = job
                    };

                    var recruiter = this.data.Recruiters
                        .FirstOrDefault(r => r.Candidates.Any(c => c.FirstName == candidate.FirstName));

                    recruiter.FreeInterviewSlots--;
                    recruiter.ExperienceLevel++;

                    this.data.Interviews.Add(interview);
                }
            }

            this.data.Jobs.Add(job);

            this.data.SaveChanges();

            return Redirect("/Jobs/All");
        }

        public HttpResponse Details(string id)
        {
            var job = this.data
                .Jobs
                .Where(j => j.Id == id)
                .Select(j => new JobListingViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Salary = j.Salary,
                    JobSkills = j.JobSkills
                })
                .FirstOrDefault();

            return View(job);
        }

        [HttpPost]
        public HttpResponse Delete(string id)
        {
            var job = this.data
                .Jobs
                .Where(j => j.Id == id)
                .Select(j => new JobListingViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Salary = j.Salary,
                    JobSkills = j.JobSkills
                })
                .FirstOrDefault();

            return View(job);
        }

    }
}
