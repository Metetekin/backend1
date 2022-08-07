using AutoMapper;

using BullBeez.Core.DTO.CompanyAndPersonDTO;
using BullBeez.Core.Entities;
using BullBeez.Core.RequestDTO;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.ResponseDTO.WebUIResponse;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BullBeez.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<CompanyAndPerson, NewUserContract>();
            CreateMap<CompanyAndPerson, UserListResponse>();
            CreateMap<Posts, PostListResponse>();

            CreateMap<Education, EducationResponse>()
                .ForMember(o => o.BeginDate, b => b.MapFrom(z => z.BeginDate.ToString("dd/MM/yyyy")))
                .ForMember(o => o.EndDate, b => b.MapFrom(z => z.EndDate.Value.ToString("dd/MM/yyyy")))
                .ForMember(o => o.UserId, b => b.MapFrom(z => z.CompanyAndPerson.Id));

            CreateMap<Project, ProjectResponse>()
               .ForMember(o => o.BeginDate, b => b.MapFrom(z => z.BeginDate.ToString("dd/MM/yyyy")))
               .ForMember(o => o.MonthCount, b => b.MapFrom(z => z.EndDate > Convert.ToDateTime("2049-01-01") ? (DateTime.Now - Convert.ToDateTime(z.BeginDate)).Days / 30 : z.MonthCount))
               .ForMember(o => o.EndDate, b => b.MapFrom(z => z.EndDate.Value. ToString("dd/MM/yyyy")));

            CreateMap<CompanyAndPerson, SearchUserByFilterResponse>()
                .ForMember(o => o.UserId, b => b.MapFrom(z => z.Id))
                .ForMember(o => o.Occupation, b => b.MapFrom(z => z.CompanyAndPersonOccupation.FirstOrDefault().Occupation.Name))
                .ForMember(o => o.CompanyTypeId, b => b.MapFrom(z => z.CompanyType.Id))
                .ForMember(o => o.CompanyTypeName, b => b.MapFrom(z => z.CompanyType.Name))
                .ForMember(o => o.Interests, b => b.MapFrom(z => String.Join(",", z.CompanyAndPersonInterests.Select(y=> "#" + y.Interest.Id + "#"))))
                .ForMember(o => o.ProfileImage, b => b.MapFrom(z => string.IsNullOrEmpty(z.ProfileImage) == true ? "https://i.hizliresim.com/7dstzi.jpg" : z.ProfileImage));

            CreateMap<Interest, InterestListResponse>()
                .ForMember(o=> o.HashId, b=> b.MapFrom(z=> "#"+z.Id+"#"));

            CreateMap<Skill, SkillResponse>()
                .ForMember(o => o.HashId, b => b.MapFrom(z => "#" + z.Id + "#"));

            CreateMap<Notification, UserNotificationResponse>().ForMember(o => o.UserId, b => b.MapFrom(z => z.CompanyAndPerson.Id))
                .ForMember(o => o.ProfileImage, b => b.MapFrom(z => string.IsNullOrEmpty(z.ProfileImage) == true ? "https://i.hizliresim.com/7dstzi.jpg" : z.ProfileImage ));
        }
    }
}