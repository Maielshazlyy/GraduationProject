using digital_employee.Controllers;
using Domain_layer.Constants;
using Domain_layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service_layer.DTOS.Auth;
using Service_layer.ServicesInterfaces;
using Xunit;

namespace Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock = new();
        private readonly Mock<UserManager<User>> _userManagerMock;

        public AuthControllerTests()
        {
            var storeMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                storeMock.Object, null, null, null, null, null, null, null, null);
        }

        private AuthController CreateController() =>
            new(_authServiceMock.Object, _userManagerMock.Object);

        private static AuthResponseDTO FakeToken(string email = "test@test.com") =>
            new() { Token = "jwt.token.here", Email = email, Role = "Agent", UserId = "u-1" };

        // ── Register ─────────────────────────────────────────────────────────

        [Fact]
        public async Task Register_ValidData_Returns200WithToken()
        {
            _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<RegisterDTO>()))
                            .ReturnsAsync(FakeToken());

            var result = await CreateController().Register(new RegisterDTO
            {
                FullName   = "Agent",
                Email      = "agent@test.com",
                Password   = "Pass123!",
                BusinessId = "biz-1"
            }) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Register_ServiceThrows_Returns400()
        {
            _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<RegisterDTO>()))
                            .ThrowsAsync(new Exception("Email already registered."));

            var result = await CreateController().Register(new RegisterDTO
            {
                Email    = "taken@test.com",
                Password = "Pass123!"
            }) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        // ── Login ─────────────────────────────────────────────────────────────

        [Fact]
        public async Task Login_ValidCredentials_Returns200WithToken()
        {
            _authServiceMock.Setup(s => s.LoginAsync(It.IsAny<LoginDTO>()))
                            .ReturnsAsync(FakeToken());

            var result = await CreateController().Login(new LoginDTO
            {
                Email    = "test@test.com",
                Password = "Pass123!"
            }) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var body = result.Value as AuthResponseDTO;
            Assert.Equal("jwt.token.here", body!.Token);
        }

        [Fact]
        public async Task Login_WrongCredentials_Returns400()
        {
            _authServiceMock.Setup(s => s.LoginAsync(It.IsAny<LoginDTO>()))
                            .ThrowsAsync(new Exception("Invalid Email or Password"));

            var result = await CreateController().Login(new LoginDTO
            {
                Email    = "test@test.com",
                Password = "wrong"
            }) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        // ── RegisterWithBusiness ──────────────────────────────────────────────

        [Fact]
        public async Task RegisterWithBusiness_ValidData_Returns200()
        {
            _authServiceMock.Setup(s => s.RegisterWithBusinessAsync(It.IsAny<RegisterWithBusinessDTO>()))
                            .ReturnsAsync(FakeToken("owner@test.com"));

            var result = await CreateController().RegisterWithBusiness(new RegisterWithBusinessDTO
            {
                FullName     = "Owner",
                Email        = "owner@test.com",
                Password     = "Pass123!",
                BusinessName = "My Restaurant"
            }) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task RegisterWithBusiness_EmailTaken_Returns400()
        {
            _authServiceMock.Setup(s => s.RegisterWithBusinessAsync(It.IsAny<RegisterWithBusinessDTO>()))
                            .ThrowsAsync(new Exception("Email already registered."));

            var result = await CreateController().RegisterWithBusiness(new RegisterWithBusinessDTO
            {
                FullName     = "Owner",
                Email        = "taken@test.com",
                Password     = "Pass123!",
                BusinessName = "Biz"
            }) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        // ── PromoteToOwner ────────────────────────────────────────────────────

        [Fact]
        public async Task PromoteToOwner_UserNotFound_Returns404()
        {
            _userManagerMock.Setup(m => m.FindByIdAsync("ghost")).ReturnsAsync((User?)null);

            var result = await CreateController().PromoteToOwner("ghost") as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task PromoteToOwner_ValidUser_Returns200WithMessage()
        {
            var user = new User { Id = "u-1", Email = "user@test.com", Role = Roles.Agent };
            _userManagerMock.Setup(m => m.FindByIdAsync("u-1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await CreateController().PromoteToOwner("u-1") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(Roles.Owner, user.Role);
        }

        [Fact]
        public async Task PromoteToOwner_UpdateFails_Returns400()
        {
            var user = new User { Id = "u-2", Email = "user2@test.com" };
            _userManagerMock.Setup(m => m.FindByIdAsync("u-2")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(user))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed." }));

            var result = await CreateController().PromoteToOwner("u-2") as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        // ── PromoteToAdmin ────────────────────────────────────────────────────

        [Fact]
        public async Task PromoteToAdmin_ValidUser_Returns200AndSetsAdminRole()
        {
            var user = new User { Id = "u-3", Email = "user3@test.com", Role = Roles.Owner };
            _userManagerMock.Setup(m => m.FindByIdAsync("u-3")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await CreateController().PromoteToAdmin("u-3") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(Roles.Admin, user.Role);
        }

        [Fact]
        public async Task PromoteToAdmin_UserNotFound_Returns404()
        {
            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            var result = await CreateController().PromoteToAdmin("ghost") as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
