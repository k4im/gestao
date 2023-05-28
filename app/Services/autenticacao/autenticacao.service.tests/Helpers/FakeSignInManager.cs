using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace autenticacao.service.tests.Helpers
{
    public class FakeSignInManager : SignInManager<AppUser>
    {
               #region Fields
        private readonly bool _simulateSuccess = false;
        #endregion

        #region Constructors
        public FakeSignInManager(bool simulateSuccess = true)
                : base(new Mock<FakeUserManager>().Object,
                     new Mock<IHttpContextAccessor>().Object,
                     new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
                     new Mock<IOptions<IdentityOptions>>().Object,
                     new Mock<ILogger<SignInManager<AppUser>>>().Object,
                     new Mock<IAuthenticationSchemeProvider>().Object,
                     new Mock<IUserConfirmation<AppUser>>().Object)
        {
            this._simulateSuccess = simulateSuccess;
        }
        #endregion

        #region Public methods
        public override Task<SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return this.ReturnResult(this._simulateSuccess);
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return this.ReturnResult(this._simulateSuccess);
        }

        public override Task<SignInResult> CheckPasswordSignInAsync(AppUser user, string password, bool lockoutOnFailure)
        {
            return this.ReturnResult(this._simulateSuccess);
        }
        #endregion

        #region Internal methods
        private Task<SignInResult> ReturnResult(bool isSuccess = true)
        {
            SignInResult result = SignInResult.Success;

            if (!isSuccess)
                result = SignInResult.Failed;

            return Task.FromResult(result);
        }
        #endregion
    }
}