using Microsoft.CodeAnalysis.CSharp.Syntax;
using TI_Net2025_DemoAspMvc.Models.Dtos.Author;
using TI_Net2025_DemoAspMvc.Models.Entities;

namespace TI_Net2025_DemoAspMvc.Mappers
{
    public static class AuthorMappers
    {
        public static AuthorDto ToAuthorDto(this Author a)
        {
            return new AuthorDto()
            {
                Id = a.Id,
                Lastname = a.Lastname,
                Firstname = a.Firstname,
            };
        }
    }
}
