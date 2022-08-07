using BullBeez.Core.Repositories;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyAndPersonRepository CompanyAndPersons { get; }

        ITokenRepository Tokens { get; }
        IMailApprovedCodeRepository MailApprovedCodes { get; }
        IOccupationRepository Occupations{ get; }
        IEducationRepository Educations { get; }
        IInterestsRepository Interests { get; }
        IProjectRepository Project { get; }
        ISkillRepository Skills { get; }
        IFollowsRepository Follows { get; }
        INotificationRepository Notifications { get; }
        IComponyAndPersonOccupationRepository ComponyAndPersonOccupationRepository { get; }
        ICityRepository City { get; }
        ICountyRepository County { get; }
        IAddressRepository Address { get; }
        ICompanyTypeRepository CompanyType { get; }
        ICompanyLevelRepository CompanyLevel { get; }
        IServiceRepository Service { get; }
        IBullBeezConfigRepository BullBeezConfig { get; }
        IPostsRepository Posts { get; }
        IUserPostsRepository UserPosts { get; }
        IPostCommentsRepository PostComments { get; }
        IFeedbackRepository Feedback { get; }
        ICompanyAndPersonInterestRepository CompanyAndPersonInterest { get; }

        ICompanyAndPersonSkillRepository CompanyAndPersonSkill { get; }
        ICompanyAndPersonProjectRepository CompanyAndPersonProject { get; }
        IServiceAnswersRepository ServiceAnswers { get; }
        IRequestHistoryRepository RequestHistory { get; }
        IPopUpReadRepository PopUpRead { get; }
        IPopUpRepository PopUp { get; }
        IFileRepository FileRepository { get; }
        IPackagePaymentRepository PackagePayment { get; }
        IPackageRepository Package { get; }
        Task<int> CommitAsync();
    }
}
