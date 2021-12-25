
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

            if (string.IsNullOrEmpty(model.Title) ||
                string.IsNullOrEmpty(model.Description) || 
                string.IsNullOrEmpty(model.Skill) ||
                string.IsNullOrEmpty(model.InterviewDate))
            {
                return Error("All fields are required!");
            }

            if (model.Salary <= 0)
            {
                return Error("Salary must be number above zero!");
            }

            var job = new Job
            {  
                Title = model.Title,
                Description = model.Description,
                Salary = model.Salary  
            };

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

            var jobSkills = job.JobSkills.Select(j => j.Name).ToList();

            var candidates = this.data.Candidates
                .Where(c => c.CandidateSkills.Any(s => jobSkills.Contains(s.Name))).ToList();

            if (candidates.Count > 0)
            {
                foreach (var candidate in candidates)
                {
                    var recruiter = this.data.Recruiters
                        .FirstOrDefault(r => r.Candidates.Any(c => c.FirstName == candidate.FirstName));

                    if (recruiter.FreeInterviewSlots > 0)
                    {
                        var interview = new Interview
                        {
                            Date = model.InterviewDate,
                            CandidateName = candidate.FirstName + ' ' + candidate.LastName,
                            RecruiterName = candidate.Recruiter.Name,
                            JobName = job.Title
                        };

                        recruiter.FreeInterviewSlots--;
                        recruiter.ExperienceLevel++;

                        this.data.Interviews.Add(interview);
                    }                   
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

        public HttpResponse Delete(string id)
        {
            var job = this.data
                .Jobs
                .Where(j => j.Id == id)
                .FirstOrDefault();

            foreach (var skill in this.data.JobsSkills)
            {
                if (skill.JobId == id)
                {
                    this.data.JobsSkills.Remove(skill);
                }
            }

            foreach (var interview in this.data.Interviews)
            {
                if (interview.JobName == job.Title)
                {
                    var recruiter = this.data.Recruiters
                        .FirstOrDefault(r => r.Name == interview.RecruiterName);
                    recruiter.FreeInterviewSlots++;
                    this.data.Interviews.Remove(interview);  
                }
            }

            this.data.Jobs.Remove(job);
            this.data.SaveChanges();

            return Redirect("/Jobs/All");
        }

    }
}
