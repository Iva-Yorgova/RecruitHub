using MyWebServer.Controllers;
using MyWebServer.Http;
using Recrutment.Data;
using Recrutment.Data.Models;
using Recrutment.ViewModels.Candidates;
using Recrutment.ViewModels.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recrutment.Controllers
{
    public class SkillsController : Controller
    {
        private readonly RecrutmentDbContext data;

        public SkillsController(RecrutmentDbContext data)
            => this.data = data;

        public HttpResponse All()
        {
            var skills = this.data
                .Skills
                .OrderBy(s => s.Name)
                .Select(s => new SkillListingViewModel
                {
                    Id = s.Id,
                    Name = s.Name,     
                    CandidatesNumber = this.data.Candidates
                    .Where(c => c.CandidateSkills.Any(sk => sk.Name == s.Name))
                    .Count(),
                    JobsNumber = this.data.Jobs
                    .Where(c => c.JobSkills.Any(sk => sk.Name == s.Name))
                    .Count()
                })
                .ToList();

            return View(skills);
        }

        public HttpResponse Active()
        {
            var activeSkills = this.data
                .CandidatesSkills
                .OrderBy(s => s.Name)
                .Select(s => new SkillListingViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    CandidatesNumber = this.data.Candidates
                    .Where(c => c.CandidateSkills.Any(sk => sk.Name == s.Name))
                    .Count(),
                    JobsNumber = this.data.Jobs
                    .Where(c => c.JobSkills.Any(sk => sk.Name == s.Name))
                    .Count()
                })
                .Where(s => s.CandidatesNumber > 0)
                .ToList();

            return View(activeSkills);
        }

        public HttpResponse Add()
        {
            return View();
        }

        [HttpPost]
        public HttpResponse Add(AddSkillFormModel model, string id)
        {

            var candidate = this.data.Candidates
                .FirstOrDefault(candidate => candidate.Id == id);

            var job = this.data.Jobs
                .FirstOrDefault(job => job.Id == id);

            var jobSkill = this.data.JobsSkills.FirstOrDefault(s => s.Name == model.Name);

            var candidateSkill = this.data.CandidatesSkills.FirstOrDefault(s => s.Name == model.Name);

            var skill = this.data.Skills.FirstOrDefault(s => s.Name == model.Name);

            if (id == null)
            {
                if (skill != null)
                {
                    return Error("Skill already exists!");
                }
                else
                {
                    this.data.Skills.Add(new Skill { Name = model.Name });
                    this.data.SaveChanges();
                    return Redirect("/Skills/All");
                }                
            }
            else
            {
                if (candidate == null)
                {
                    if (job.JobSkills.Contains(jobSkill))
                    {
                        return Error("Skill already added!");
                    }
                    else
                    {
                        var jskill = new JobSkill { Name = model.Name };
                        job.JobSkills.Add(jskill);
                        if (!this.data.Skills.Any(s => s.Name == jskill.Name))
                        {
                            this.data.Skills.Add(new Skill { Name = model.Name });
                            this.data.SaveChanges();
                        }
                        this.data.SaveChanges();
                        return Redirect("/Jobs/All");
                    }           
                }
                else
                {
                    if (candidate.CandidateSkills.Contains(candidateSkill))
                    {
                        return Error("Skill already added!");
                    }
                    else
                    {
                        var cskill = new CandidateSkill { Name = model.Name };
                        candidate.CandidateSkills.Add(cskill);
                        if (!this.data.Skills.Any(s => s.Name == cskill.Name))
                        {
                            this.data.Skills.Add(new Skill { Name = model.Name });
                            this.data.SaveChanges();
                        }
                        this.data.SaveChanges();
                        return Redirect("/Candidates/All");
                    }              
                }
            }
        }
    }
}
