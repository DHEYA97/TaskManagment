using TaskManagment.Core.Entities;
using TaskManagment.Core.ViewModel;
using TaskManagment.Core.ViewModel.User;

namespace TaskManagment.Mvc.Mapping
{
    public class MappingConfiguration() : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            
            config.NewConfig<UserFormViewModel, ApplicationUser>()
                .Map(dest => dest.NormalizedEmail, src => src.Email.ToUpper())
                .Map(dest => dest.NormalizedUserName, src => src.UserName.ToUpper())
                .TwoWays();
        }
    }
}
