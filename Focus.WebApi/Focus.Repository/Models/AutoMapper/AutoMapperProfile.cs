using AutoMapper;

namespace Focus.Repository.Models
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SysUser, RegisterSysUser>();
            CreateMap<RegisterSysUser, SysUser>();
        }
    }
}
