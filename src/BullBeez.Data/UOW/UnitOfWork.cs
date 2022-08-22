using BullBeez.Core.Repositories;
using BullBeez.Core.UOW;
using BullBeez.Data.Context;
using BullBeez.Data.Repositories;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Data.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BullBeezDBContext _context;
        private CompanyAndPersonRepository _companyAndPersonRepository;
        private TokenRepository _tokenRepository;
        private MailApprovedCodeRepository _mailApprovedCodeRepository;
        private IOccupationRepository _occupationRepository;
        private IEducationRepository _educationRepository;
        private IInterestsRepository _interestsRepository;
        private IProjectRepository _projectRepository;
        private ISkillRepository _skillRepository;
        private IFollowsRepository _followsRepository;
        private INotificationRepository _notificationRepository;
        private IComponyAndPersonOccupationRepository _omponyAndPersonOccupationRepository;
        private ICityRepository _cityRepository;
        private ICountyRepository _countyRepository;
        private IAddressRepository _addressRepository;
        private ICompanyTypeRepository _companyTypeRepository;
        private ICompanyLevelRepository _companyLevelRepository;
        private IServiceRepository _serviceRepository;
        private IBullBeezConfigRepository _bullBeezConfigRepository;
        private IPostsRepository _postsRepository;
        private IFeedbackRepository _feedbackRepository;
        private ICompanyAndPersonInterestRepository _companyAndPersonInterestRepository;
        private ICompanyAndPersonSkillRepository _companyAndPersonSkillRepository;
        private ICompanyAndPersonProjectRepository _companyAndPersonProjectRepository;
        private IFileRepository _fileRepository;
        private IServiceAnswersRepository _serviceAnswersRepository;
        private IRequestHistoryRepository _requestHistoryRepository;
        private IPopUpReadRepository _popUpReadRepository;
        private IPopUpRepository _popUpRepository;
        private IUserPostsRepository _userPostsRepository;
        private IPostCommentsRepository _postCommentsRepository;
        private IPackagePaymentRepository _packagePaymentRepository;
        private IPackageRepository _packageRepository;
        private Dictionary<Type, object> repositories;
        private IPostReportRepository _postReportRepository;
        public UnitOfWork(BullBeezDBContext context)
        {
            this._context = context;
        }

        public ICompanyAndPersonRepository CompanyAndPersons => _companyAndPersonRepository = _companyAndPersonRepository ?? new CompanyAndPersonRepository(_context);
        public ITokenRepository Tokens => _tokenRepository = _tokenRepository ?? new TokenRepository(_context);
        public IMailApprovedCodeRepository MailApprovedCodes => _mailApprovedCodeRepository = _mailApprovedCodeRepository ?? new MailApprovedCodeRepository(_context);
        public IOccupationRepository Occupations => _occupationRepository = _occupationRepository ?? new OccupationRepository(_context);

        public IEducationRepository Educations => _educationRepository = _educationRepository ?? new EducationRepository(_context);

        public IInterestsRepository Interests => _interestsRepository = _interestsRepository ?? new InterestsRepository(_context);

        public IProjectRepository Project => _projectRepository = _projectRepository ?? new ProjectRepository(_context);

        public ISkillRepository Skills => _skillRepository = _skillRepository ?? new SkillRepository(_context);
        public IFollowsRepository Follows => _followsRepository = _followsRepository ?? new FollowsRepository(_context);

        public INotificationRepository Notifications => _notificationRepository = _notificationRepository ?? new NotificationRepository(_context);

        public IComponyAndPersonOccupationRepository ComponyAndPersonOccupationRepository => _omponyAndPersonOccupationRepository = _omponyAndPersonOccupationRepository ?? new ComponyAndPersonOccupationRepository(_context);

        public ICityRepository City => _cityRepository = _cityRepository ?? new CityRepository(_context);

        public ICountyRepository County => _countyRepository = _countyRepository ?? new CountyRepository(_context);

        public IAddressRepository Address => _addressRepository = _addressRepository ?? new AddressRepository(_context);

        public ICompanyTypeRepository CompanyType => _companyTypeRepository = _companyTypeRepository ?? new CompanyTypeRepository(_context);

        public IServiceRepository Service => _serviceRepository = _serviceRepository ?? new ServiceRepository(_context);

        public IBullBeezConfigRepository BullBeezConfig => _bullBeezConfigRepository = _bullBeezConfigRepository ?? new BullBeezConfigRepository(_context);

        public IPostsRepository Posts => _postsRepository = _postsRepository ?? new PostsRepository(_context);

        public IFeedbackRepository Feedback => _feedbackRepository = _feedbackRepository ?? new FeedbackRepository(_context);

        public ICompanyAndPersonInterestRepository CompanyAndPersonInterest => _companyAndPersonInterestRepository = _companyAndPersonInterestRepository ?? new CompanyAndPersonInterestRepository(_context);

        public ICompanyAndPersonSkillRepository CompanyAndPersonSkill => _companyAndPersonSkillRepository = _companyAndPersonSkillRepository ?? new CompanyAndPersonSkillRepository(_context);

        public ICompanyAndPersonProjectRepository CompanyAndPersonProject => _companyAndPersonProjectRepository = _companyAndPersonProjectRepository ?? new CompanyAndPersonProjectRepository(_context);

        public ICompanyLevelRepository CompanyLevel => _companyLevelRepository = _companyLevelRepository ?? new CompanyLevelRepository(_context);

        public IFileRepository FileRepository => _fileRepository = _fileRepository ?? new FileRepository(_context);

        public IServiceAnswersRepository ServiceAnswers => _serviceAnswersRepository = _serviceAnswersRepository ?? new ServiceAnswersRepository(_context);

        public IRequestHistoryRepository RequestHistory => _requestHistoryRepository = _requestHistoryRepository ?? new RequestHistoryRepository(_context);

        public IPopUpReadRepository PopUpRead => _popUpReadRepository = _popUpReadRepository ?? new PopUpReadRepository(_context);

        public IPopUpRepository PopUp => _popUpRepository = _popUpRepository ?? new PopUpRepository(_context);

        public IUserPostsRepository UserPosts => _userPostsRepository = _userPostsRepository ?? new UserPostsRepository(_context);

        public IPostCommentsRepository PostComments => _postCommentsRepository = _postCommentsRepository ?? new PostCommentsRepository(_context);

        public IPackagePaymentRepository PackagePayment => _packagePaymentRepository = _packagePaymentRepository ?? new PackagePaymentRepository(_context);

        public IPackageRepository Package => _packageRepository = _packageRepository ?? new PackageRepository(_context);
        
        public IPostReportRepository PostReport => _postReportRepository = _postReportRepository ?? new PostReportRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            _context.Dispose();
        }
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(_context);
            }

            return (IRepository<TEntity>)repositories[type];
        }
    }
}
