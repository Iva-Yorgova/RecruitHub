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
                    CandidateSkills = c.CandidateSkills
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
            // Validation!

            var recruiter = new Recruiter
            {
                Name = model.RecruiterName,
                Epost = model.RecruiterEmail,
                Country = model.RecruiterCountry
            };

            var candidate = new Candidate
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Bio = model.Bio,
                Recruiter = recruiter
            };

            if (model.Skill != null)
            {
                if (!this.data.Skills.Any(s => s.Name == model.Skill))
                {
                    var skill = new CandidateSkill { Name = model.Skill };
                    candidate.CandidateSkills.Add(skill);
                    this.data.Skills.Add(new Skill { Name = model.Skill });
                }
                else
                {
                    candidate.CandidateSkills.Add(new CandidateSkill { Name = model.Skill });
                }               
            }        

            recruiter.Candidates.Add(candidate);

            this.data.Candidates.Add(candidate);
            this.data.Recruiters.Add(recruiter);

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
                    Bio = c.Bio
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

    }
}
