using Banderas.Web.Business.UseCases.Flags;
using Banderas.Web.Business.UserInfo;
using Banderas.Web.Data;
using Banderas.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banderas.UnitTests.Web.Business.UseCases.Flags
{
    public class FlagUserDetailsStub : IFlagUserDetails
    {
        public string UserId => "1";
    }
    public class AddFlagUseCaseTest
    {

        [Fact]
        public async Task WhenFlagNameAlreadyExist_ThenError()
        {
            // Arrange
            IFlagUserDetails userDetails = new FlagUserDetailsStub();
            ApplicationDbContext contextInMemory = GetInMemoryDbContext(userDetails);
            FlagEntity flag = new FlagEntity
            {
                UserId = userDetails.UserId,
                Name = "Test",
                Value = true
            };
            
            contextInMemory.Flags.Add(flag);
            await contextInMemory.SaveChangesAsync();

            //Act
            AddFlagUseCase addFlagUseCase = new AddFlagUseCase(contextInMemory, userDetails);
            var result = await addFlagUseCase.Execute(flag.Name, flag.Value);

            //Assert
            Assert.False(result.Success);
            Assert.Equal($"Flag {flag.Name} already exists", result.Errors.First().Message);
        }

        [Fact]
        public async Task WhenFlagDoesNotExist_ThenInsertedOnDb()
        {
            //arrange
            IFlagUserDetails flagUserDetails = new FlagUserDetailsStub();
            ApplicationDbContext inMemoryDb = GetInMemoryDbContext(flagUserDetails);
            
            //act
            AddFlagUseCase addFlagUseCase = new AddFlagUseCase(inMemoryDb, flagUserDetails);
            var result = await addFlagUseCase.Execute("SomeName", true);
            
            //assert
            Assert.True(result.Success);
            Assert.True(result.Value);
        }
        private ApplicationDbContext GetInMemoryDbContext(IFlagUserDetails userDetails)
        {
            DbContextOptions<ApplicationDbContext> databaseOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "flagTest")
                .Options;

            return new ApplicationDbContext(databaseOptions, userDetails);
        }
    }
}