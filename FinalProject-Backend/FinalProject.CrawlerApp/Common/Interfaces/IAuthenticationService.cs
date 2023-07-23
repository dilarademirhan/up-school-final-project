using CrawlerApp.Common.Models.Auth;

namespace CrawlerApp.Common.Interfaces
{
    public interface IAuthenticationService
    {
       Task<JwtDto> SocialLoginAsync(string email, string firstName, string lastName, CancellationToken cancellationToken);
    }
}