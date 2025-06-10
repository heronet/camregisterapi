using src.Data.DTO;
using src.Models;

namespace src.Extensions;

public static class DtoConverter
{
    public static UserDto ToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
        };
    }

}
