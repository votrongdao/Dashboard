using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using DashboardSite.Model;
using DashboardSite.Core;
using EntityObject.Entities.EntityFromDB;

namespace DashboardSite.Repository
{
    public static class AutoMapperRepositoryConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<usp_AuthenticateInitial_LogIn_Result, UserInfoModel>();
                cfg.CreateMap<Address, AddressModel>();
                cfg.CreateMap<AddressModel, Address>();
                cfg.CreateMap<Authentication, UserViewModel>()
                    .ForMember(dest => dest.SelectedTimezoneId, map => map.MapFrom(src => src.PreferredTimeZone))
                    .ForMember(dest => dest.SelectedRoles, map => map.MapFrom(src => src.RoleName));
                cfg.CreateMap<UserViewModel, Authentication>()
                    .ForMember(dest => dest.PreferredTimeZone, map => map.MapFrom(src => src.SelectedTimezoneId));
                cfg.CreateMap<Email, EmailModel>();
                cfg.CreateMap<EmailModel, Email>();
                cfg.CreateMap<SecurityQuestion, SecurityQuestionLookup>();
                cfg.CreateMap<SecurityQuestionLookup, SecurityQuestion>();
                cfg.CreateMap<EmailTypeLookup, EmailType>();
                cfg.CreateMap<EmailType, EmailTypeLookup>();                
            });
            //Mapper.AssertConfigurationIsValid();
        }


        

        //private static void ConfigureUploadDataIntermediateMapping()
        //{
        //    Mapper.CreateMap<IEnumerable<Lender_DataUpload_Intermediate>, IEnumerable<ExcelUploadView_Model>>();
        //    Mapper.CreateMap<IEnumerable<ExcelUploadView_Model>, IEnumerable<Lender_DataUpload_Intermediate>>();
        //}

        //private static void ConfigureUploadViewResultMapping()
        //{
        //    Mapper.CreateMap<IEnumerable<usp_Get_ExcelUpload_View_Result>, IEnumerable<ReportExcelUpload_Model>>();
        //}

        //private static void ConfigureUserInfoMapping()
        //{
        //    Mapper.CreateMap<usp_AuthenticateInitial_LogIn_Result, UserInfoModel>();
        //}
    }
}
