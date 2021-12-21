
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

                //var skill = this.data.JobsSkills
                //.FirstOrDefault(s => s.Name == model.Skill);
                //
                //if (skill != null)
                //{
                //    job.JobSkills.Add(skill);
                //}
                //else
                //{
                //    job.JobSkills.Add(new JobSkill { Name = model.Skill });
                //}
            }

                

            this.data.Jobs.Add(job);

            this.data.SaveChanges();

            return Redirect("/Jobs/All");
        }

    }
}
