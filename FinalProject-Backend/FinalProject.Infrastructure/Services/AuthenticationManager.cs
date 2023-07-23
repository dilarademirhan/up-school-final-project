using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using CrawlerApp.Common.Interfaces;
using CrawlerApp.Common.Models;
using Domain.Identity;
using CrawlerApp.Common.Models.Auth;
using Microsoft.AspNetCore.Authentication;

namespace FinalProject.Infrastructure.Services
{
    public class AuthenticationManager : CrawlerApp.Common.Interfaces.IAuthenticationService
    {

        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        /*    private readonly SignInManager<User> _signInManager;
            private readonly IStringLocalizer<CommonLocalizations> _localizer; */


        public AuthenticationManager(UserManager<User> userManager, SignInManager<User> signInManager, 
            IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;

            /* _signInManager = signInManager;
             _localizer = localizer;*/
        }

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public async Task<JwtDto> SocialLoginAsync(string email, string firstName, string lastName, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is not null)
                return _jwtService.Generate(user.Id, user.Email, user.FirstName, user.LastName);

            var userId = Guid.NewGuid().ToString();

            user = new User()
            {
                Id = userId,
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                CreatedOn = DateTimeOffset.Now,
                CreatedByUserId = userId,
            };

            var identityResult = await _userManager.CreateAsync(user);

            if (!identityResult.Succeeded)
            {
                var failures = identityResult.Errors
                    .Select(x => new ValidationFailure(x.Code, x.Description));

                throw new FluentValidation.ValidationException(failures);
            }

            return _jwtService.Generate(user.Id, user.Email, user.FirstName, user.LastName);
        }

        Task<JwtDto> CrawlerApp.Common.Interfaces.IAuthenticationService.SocialLoginAsync(string email, string firstName, string lastName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}