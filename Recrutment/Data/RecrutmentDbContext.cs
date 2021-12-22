namespace Recrutment.Data
{
 
    using Microsoft.EntityFrameworkCore;
    using Recrutment.Data.Models;

    public class RecrutmentDbContext : DbContext
    {
        
        public DbSet<Candidate> Candidates { get; set; }
        
        public DbSet<Job> Jobs { get; set; }
        
        public DbSet<Recruiter> Recruiters { get; set; }

        public DbSet<Interview> Interviews { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<CandidateSkill> CandidatesSkills { get; set; }

        public DbSet<JobSkill> JobsSkills { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Recrutment;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
                .Entity<Candidate>()
                .HasOne(c => c.Recruiter)
                .WithMany(r => r.Candidates)
                .HasForeignKey(r => r.RecruiterId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<Interview>()
                .HasOne(i => i.Recruiter)
                .WithMany(r => r.Interviews)
                .HasForeignKey(r => r.RecruiterId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}