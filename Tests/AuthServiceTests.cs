using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Service_layer.DTOS.Auth;
using Service_layer.Services;
using Xunit;

namespace Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IBusinessRepository> _businessRepoMock = new();
        private readonly IConfiguration _config;

        public AuthServiceTests()
        {
            var storeMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                storeMock.Object, null, null, null, null, null, null, null, null);

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["JWT:Key"]      = "super-secret-key-for-testing-1234567890ab",
                    ["JWT:Issuer"]   = "TestIssuer",
                    ["JWT:Audience"] = "TestAudience"
                })
                .Build();

            _uowMock.Setup(u => u.Businesses).Returns(_businessRepoMock.Object);
        }

        private AuthService CreateService() =>
            new(_userManagerMock.Object, _config, _uowMock.Object);

        private static User FakeUser(string id = "user-1", string email = "test@test.com") =>
            new() { Id = id, Email = email, FullName = "Test User", Role = "Agent", BusinessId = "biz-1" };

        // ── LoginAsync ───────────────────────────────────────────────────────

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsTokenResponse()
        {
            var user = FakeUser();
            _userManagerMock.Setup(m => m.FindByEmailAsync("test@test.com")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.CheckPasswordAsync(user, "Pass123!")).ReturnsAsync(true);

            var service = CreateService();
            var result  = await service.LoginAsync(new LoginDTO { Email = "test@test.com", Password = "Pass123!" });

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
            Assert.Equal("test@test.com", result.Email);
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ThrowsException()
        {
            var user = FakeUser();
            _userManagerMock.Setup(m => m.FindByEmailAsync("test@test.com")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.CheckPasswordAsync(user, "wrong")).ReturnsAsync(false);

            var service = CreateService();
            await Assert.ThrowsAsync<Exception>(() =>
                service.LoginAsync(new LoginDTO { Email = "test@test.com", Password = "wrong" }));
        }

        [Fact]
        public async Task LoginAsync_UserNotFound_ThrowsException()
        {
            _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<Exception>(() =>
                service.LoginAsync(new LoginDTO { Email = "ghost@test.com", Password = "Pass123!" }));
        }

        // ── RegisterAsync ────────────────────────────────────────────────────

        [Fact]
        public async Task RegisterAsync_ValidData_ReturnsToken()
        {
            var business = new Business { Id = "biz-1", BusinessId = "biz-1", Name = "Biz" };

            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(business);
            _userManagerMock.Setup(m => m.FindByEmailAsync("new@test.com")).ReturnsAsync((User?)null);
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            var service = CreateService();
            var result  = await service.RegisterAsync(new RegisterDTO
            {
                FullName   = "New Agent",
                Email      = "new@test.com",
                Password   = "Pass123!",
                BusinessId = "biz-1"
            });

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
        }

        [Fact]
        public async Task RegisterAsync_MissingBusinessId_ThrowsException()
        {
            var service = CreateService();
            await Assert.ThrowsAsync<Exception>(() =>
                service.RegisterAsync(new RegisterDTO
                {
                    FullName   = "X",
                    Email      = "x@x.com",
                    Password   = "Pass123!",
                    BusinessId = null
                }));
        }

        [Fact]
        public async Task RegisterAsync_BusinessNotFound_ThrowsException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Business?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<Exception>(() =>
                service.RegisterAsync(new RegisterDTO
                {
                    FullName   = "X",
                    Email      = "x@x.com",
                    Password   = "Pass123!",
                    BusinessId = "bad-biz"
                }));
        }

        [Fact]
        public async Task RegisterAsync_EmailAlreadyExists_ThrowsException()
        {
            var business = new Business { Id = "biz-1", BusinessId = "biz-1", Name = "Biz" };
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(business);
            _userManagerMock.Setup(m => m.FindByEmailAsync("taken@test.com")).ReturnsAsync(FakeUser());

            var service = CreateService();
            await Assert.ThrowsAsync<Exception>(() =>
                service.RegisterAsync(new RegisterDTO
                {
                    FullName   = "X",
                    Email      = "taken@test.com",
                    Password   = "Pass123!",
                    BusinessId = "biz-1"
                }));
        }

        // ── RegisterWithBusinessAsync ────────────────────────────────────────

        [Fact]
        public async Task RegisterWithBusinessAsync_ValidData_CreatesBusinessAndReturnsToken()
        {
            _userManagerMock.Setup(m => m.FindByEmailAsync("owner@test.com")).ReturnsAsync((User?)null);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            var service = CreateService();
            var result  = await service.RegisterWithBusinessAsync(new RegisterWithBusinessDTO
            {
                FullName     = "Owner",
                Email        = "owner@test.com",
                Password     = "Pass123!",
                BusinessName = "My Restaurant"
            });

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
            // Business must have been added before user creation
            _businessRepoMock.Verify(r => r.AddAsync(It.IsAny<Business>()), Times.Once);
        }

        [Fact]
        public async Task RegisterWithBusinessAsync_EmailTaken_ThrowsException()
        {
            _userManagerMock.Setup(m => m.FindByEmailAsync("taken@test.com")).ReturnsAsync(FakeUser());

            var service = CreateService();
            await Assert.ThrowsAsync<Exception>(() =>
                service.RegisterWithBusinessAsync(new RegisterWithBusinessDTO
                {
                    FullName     = "Owner",
                    Email        = "taken@test.com",
                    Password     = "Pass123!",
                    BusinessName = "My Restaurant"
                }));
        }

        [Fact]
        public async Task RegisterWithBusinessAsync_UserCreateFails_RollsBackBusiness()
        {
            _userManagerMock.Setup(m => m.FindByEmailAsync("fail@test.com")).ReturnsAsync((User?)null);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Weak password." }));

            var service = CreateService();
            await Assert.ThrowsAsync<Exception>(() =>
                service.RegisterWithBusinessAsync(new RegisterWithBusinessDTO
                {
                    FullName     = "Owner",
                    Email        = "fail@test.com",
                    Password     = "weak",
                    BusinessName = "Orphan Biz"
                }));

            // Orphan business must be deleted on failure
            _businessRepoMock.Verify(r => r.Delete(It.IsAny<Business>()), Times.Once);
        }

        // ── RegisterAdminAsync / RegisterOwnerAsync ──────────────────────────

        [Fact]
        public async Task RegisterAdminAsync_ValidData_ReturnsToken()
        {
            _userManagerMock.Setup(m => m.FindByEmailAsync("admin@test.com")).ReturnsAsync((User?)null);
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            var service = CreateService();
            var result  = await service.RegisterAdminAsync(new RegisterBootstrapDTO
            {
                FullName = "Admin",
                Email    = "admin@test.com",
                Password = "Pass123!"
            });

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
        }

        [Fact]
        public async Task RegisterAdminAsync_EmailTaken_ThrowsException()
        {
            _userManagerMock.Setup(m => m.FindByEmailAsync("taken@test.com")).ReturnsAsync(FakeUser());

            var service = CreateService();
            await Assert.ThrowsAsync<Exception>(() =>
                service.RegisterAdminAsync(new RegisterBootstrapDTO
                {
                    FullName = "Admin",
                    Email    = "taken@test.com",
                    Password = "Pass123!"
                }));
        }
    }
}
