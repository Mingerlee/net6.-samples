using AutoMapper;


namespace Focus.Repository.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainToViewModelMappingProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public DomainToViewModelMappingProfile()
        {
            //CreateMap<,>();
            CreateMap<SysUser, RegisterSysUser>();
        }
    }
}
