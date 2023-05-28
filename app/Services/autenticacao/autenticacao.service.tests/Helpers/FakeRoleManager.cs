using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace autenticacao.service.tests.Helpers
{
    public class FakeRoleManager : RoleManager<IdentityRole>
    {
        public FakeRoleManager() 
            : base(new Mock<IRoleStore<IdentityRole>>().Object, 
            new Mock<IEnumerable<IRoleValidator<IdentityRole>>>().Object, 
            new Mock<ILookupNormalizer>().Object, 
            new Mock<IdentityErrorDescriber>().Object, 
            new Mock<ILogger<RoleManager<IdentityRole>>>().Object)
        {
        }
    }
}