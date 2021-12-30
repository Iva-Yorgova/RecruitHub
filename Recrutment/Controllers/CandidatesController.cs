using MyWebServer.Controllers;
using MyWebServer.Http;
using Recrutment.Data;
using Recrutment.Data.Models;
using Recrutment.ViewModels.Candidates;
using System.Linq;


namespace Recrutment.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly RecrutmentDbContext data;

        public CandidatesController(RecrutmentDbContext data) 
            => this.data = data;

        public HttpResponse All()
        {
            var candidates = this.data
                .Candidates
                .OrderBy(c => c.FirstName)
                .Select(c => new CandidateListingViewModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    BirthDate = c.BirthDate,
                    CandidateSkills = c.CandidateSkills,
                    Recruiter = c.Recruiter,
                    Interviews = this.data.Interviews.Where(i => i.CandidateName == c.FirstName + ' ' + c.LastName).Count()
                })
                .ToList();

            return View(candidates);
        }

        public HttpResponse Create()
        {
            return View();
        }

        [HttpPost]
        public HttpResponse Create(CreateCandidateFormModel model)
        {
            if (this.data.Candidates
                .Any(c => c.FirstName == model.FirstName && 
                c.LastName == model.LastName))
            {
                return Error("Candidate already exists!");
            }

            if (string.IsNullOrEmpty(model.FirstName) || 
                string.IsNullOrEmpty(model.LastName) ||
                string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Bio) ||
                string.IsNullOrEmpty(model.BirthDate) ||
                string.IsNullOrEmpty(model.Skill) ||
                string.IsNullOrEmpty(model.RecruiterName) ||
                string.IsNullOrEmpty(model.RecruiterEmail) ||
                string.IsNullOrEmpty(model.RecruiterCountry))
            {
                return Error("All fields are required!");
            }

            var recruiter = this.data.Recruiters.FirstOrDefault(r => r.Name == model.RecruiterName);

            if (recruiter == null)
            {
                recruiter = new Recruiter
                {
                    Name = model.RecruiterName,
                    Epost = model.RecruiterEmail,
                    Country = model.RecruiterCountry
                };
                this.data.Recruiters.Add(recruiter);
            }
            else
            {              
                recruiter.ExperienceLevel++;    
            }

            var candidate = new Candidate
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Bio = model.Bio,
                RecruiterId = recruiter.Id
            };

            if (!this.data.Skills.Any(s => s.Name == model.Skill))
            {
                var skill = new CandidateSkill { Name = model.Skill };
                candidate.CandidateSkills.Add(skill);
                this.data.Skills.Add(new Skill { Name = model.Skill });
            }
            else
            {
                candidate.CandidateSkills.Add(new CandidateSkill{ Name = model.Skill });
            }               

            recruiter.Candidates.Add(candidate);

            this.data.Candidates.Add(candidate);          

            this.data.SaveChanges();

            return Redirect("/Candidates/All");
        }

        public HttpResponse Details(string id)
        {
            var candidate = this.data
                .Candidates
                .Where(c => c.Id == id)
                .Select(c => new CandidateListingViewModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    BirthDate = c.BirthDate,
                    CandidateSkills = c.CandidateSkills,
                    Bio = c.Bio,
                    Interviews = this.data.Interviews.Where(i => i.CandidateName == c.FirstName + ' ' + c.LastName).Count()
                })
                .FirstOrDefault();

            return View(candidate);
        }

        public HttpResponse Edit(string id)
        {
            var candidate = this.data.Candidates
                .Where(c => c.Id == id)
                .Select(c => new CreateCandidateFormModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    BirthDate = c.BirthDate,
                    Bio = c.Bio,
                    CandidateSkills = c.CandidateSkills
                })
                .FirstOrDefault();

            return View(candidate);
        }

        [HttpPost]
        public HttpResponse Edit(CreateCandidateFormModel model)
        {
            // Validation!
            //if (!ModelState.IsValid)
            //{
            //    return View(template);
            //}
           
            var candidateData = this.data.Candidates
                .FirstOrDefault(t => t.Id == model.Id);

            if (candidateData == null)
            {
                return Error("Candidate not found.");
            }

            if (string.IsNullOrEmpty(model.FirstName) ||
                string.IsNullOrEmpty(model.LastName) ||
                string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Bio) ||
                string.IsNullOrEmpty(model.BirthDate))
            {
                return Error("All fields are required!");
            }

            candidateData.FirstName = model.FirstName;
            candidateData.LastName = model.LastName;
            candidateData.Bio = model.Bio;
            candidateData.BirthDate = model.BirthDate;
            candidateData.Email = model.Email;

            if (model.Skill != null)
            {
                candidateData.CandidateSkills.Add(new CandidateSkill { Name = model.Skill });
            }

            this.data.SaveChanges();

            return Redirect("/Candidates/All");
        }

        public HttpResponse Delete(string id)
        {
            var candidate = this.data.Candidates
                .Where(c => c.Id == id)
                .FirstOrDefault();

            var candidateSkills = this.data.CandidatesSkills
                .Where(s => s.CandidateId == id)
                .ToList();

            foreach (var skill in candidateSkills)
            {
                this.data.CandidatesSkills.Remove(skill);
            }

            this.data.Candidates.Remove(candidate);
            this.data.SaveChanges();

            return Redirect("/Candidates/All");
        }

    }
}
