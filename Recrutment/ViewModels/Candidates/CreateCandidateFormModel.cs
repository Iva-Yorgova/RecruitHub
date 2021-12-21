using Recrutment.Data.Models;
using System.Collections.Generic;

namespace Recrutment.ViewModels.Candidates
{
    public class CreateCandidateFormModel
    {
       
        public string Id { get; set; } 
    
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string BirthDate { get; set; }

        public string Skill { get; set; }

        public IEnumerable<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();

        public string RecruiterName { get; set; }

        public string RecruiterEmail { get; set; }

        public string RecruiterCountry { get; set; }
    }
}
