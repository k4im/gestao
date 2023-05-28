using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace autenticacao.service.tests.Helpers
{
    public class FakeUserManager : UserManager<AppUser>
    {
                public FakeUserManager()
            : base(new Mock<IUserStore<AppUser>>().Object,
                   new Mock<IOptions<IdentityOptions>>().Object,
                   new Mock<IPasswordHasher<AppUser>>().Object,
                   new IUserValidator<AppUser>[0],
                   new IPasswordValidator<AppUser>[0],
                   new Mock<ILookupNormalizer>().Object,
                   new Mock<IdentityErrorDescriber>().Object,
                   new Mock<IServiceProvider>().Object,
                   new Mock<ILogger<UserManager<AppUser>>>().Object)
        { }

        public override Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<IdentityResult> AddToRoleAsync(AppUser user, string role)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
    }
}