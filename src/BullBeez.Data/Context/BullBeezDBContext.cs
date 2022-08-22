using BullBeez.Core.Entities;
using BullBeez.Data.Configurations;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Context
{
    public class BullBeezDBContext : DbContext
    {
        //EntityFrameworkCore\Add-Migration Version_1.5
        //EntityFrameworkCore\update-database
        public DbSet<CompanyAndPerson> CompanyAndPersons { get; set; }
        public DbSet<CompanyAndPersonDetail> CompanyAndPersonDetails { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Follows> Follows { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<MailApprovedCode> MailApprovedCodes { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<County> County { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<QuestionOptions> QuestionOptions { get; set; }
        public DbSet<ServiceQuestion> ServiceQuestions { get; set; }
        public DbSet<CompanyType> CompanyType { get; set; }
        public DbSet<CompanyLevel> CompanyLevel { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FileData> File { get; set; }
        public DbSet<BullBeezConfig> BullBeezConfig { get; set; }
        public DbSet<ServiceAnswers> ServiceAnswer { get; set; }
        public DbSet<CompanyAndPersonInterest> CompanyAndPersonInterest { get; set; }
        public DbSet<RequestHistory> RequestHistory { get; set; }
        public DbSet<PopUp> PopUp { get; set; }
        public DbSet<PopUpIsRead> PopUpIsRead { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<UserPosts> UserPosts { get; set; }
        public DbSet<PostComments> PostComments { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackagePayments> PackagePayments { get; set; }
        public DbSet<PostReport> PostReport { get; set; }
        
        public BullBeezDBContext(DbContextOptions<BullBeezDBContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new CompanyAndPersonConfigurations());
            builder
                .ApplyConfiguration(new CompanyAndPersonDetailConfigurations());
            builder
                .ApplyConfiguration(new EducationConfigurations());
            builder
                .ApplyConfiguration(new InterestConfigurations());
            builder
                .ApplyConfiguration(new OccupationConfigurations());
            builder
                .ApplyConfiguration(new ProjectConfigurations());
            builder
                .ApplyConfiguration(new SkillConfigurations());
            builder
                .ApplyConfiguration(new TokenConfigurations());
            builder
                .ApplyConfiguration(new TestConfigurations());
            builder
               .ApplyConfiguration(new FollowsConfigurations());
            builder
              .ApplyConfiguration(new NotificationConfiguration());
            builder
              .ApplyConfiguration(new CompanyAndPersonOccupationConfigurations());
            builder
              .ApplyConfiguration(new MailApprovedCodeConfigurations());
            builder
              .ApplyConfiguration(new CityConfigurations());
            builder
              .ApplyConfiguration(new CountyConfigurations());
            builder
              .ApplyConfiguration(new AddressConfigurations());
            builder
             .ApplyConfiguration(new CompanyTypeConfigurations());
            builder
            .ApplyConfiguration(new ServiceConfigurations());
            builder
            .ApplyConfiguration(new FeedbackConfiguration());
            builder
            .ApplyConfiguration(new PostsConfiguration());
            builder
            .ApplyConfiguration(new BullBeezConfigConfiguration());
            builder
                .ApplyConfiguration(new UserPostsConfiguration());
            builder
                .ApplyConfiguration(new PostCommentsConfiguration());
            builder
                .ApplyConfiguration(new PostReportConfigurations());
        }
    }

}
