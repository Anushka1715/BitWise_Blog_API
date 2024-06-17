using Microsoft.AspNetCore.Identity;

namespace BitWiseBlog.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
