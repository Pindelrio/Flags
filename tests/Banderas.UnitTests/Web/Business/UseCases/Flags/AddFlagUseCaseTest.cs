using Banderas.Web.Business.UserInfo;

namespace Banderas.UnitTests.Web.Business.UseCases.Flags
{
    public class AddFlagUseCaseTest
    {
        public class FlagUserDetailsStub : IFlagUserDetails
        {
            public string UserId => "1";
        }

        [Fact]
        public async Task ThenFlagNameAlreadyExist_ThenError()
        {

        }
    }
}