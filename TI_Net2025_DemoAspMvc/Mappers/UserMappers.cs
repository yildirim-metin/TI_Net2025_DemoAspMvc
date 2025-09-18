using TI_Net2025_DemoAspMvc.Models.Dtos.User;
using TI_Net2025_DemoAspMvc.Models.Entities;

namespace TI_Net2025_DemoAspMvc.Mappers;

public static class UserMappers
{
    public static RegisterFormDto ToRegisterFormDto(this User user)
    {
        return new RegisterFormDto()
        {
            Email = user.Email,
            Username = user.Username,
        };
    }

    public static User ToUser(this RegisterFormDto registerFormDto)
    {
        return new User()
        {
            Email = registerFormDto.Email,
            Username = registerFormDto.Username,
            Password = registerFormDto.Password,
        };
    }
}
