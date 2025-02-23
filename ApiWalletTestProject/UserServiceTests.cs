
using Moq;
using Xunit;
using System.Threading.Tasks;
using CustodialWallet.Domain.Abstract;
using Microsoft.Extensions.Logging;
using CustodialWallet.Application.Service;
using CustodialWallet.Application.DTO;
using CustodialWallet.Domain.Entity;
namespace ApiWalletTestProject
{
    public class UserServiceTests
    {
        [Fact]
        public async Task CreateUserAsync_ShouldReturnResponseDTO_WhenUserIsCreated()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var userService = new UserService(mockRepo.Object, mockLogger.Object);
            var createUserRequest = new CreateUserRequest { Email = "test@example.com" };

            var expectedUser = new User { UserId = Guid.NewGuid(), Email = "test@example.com", Balance = 0 };
            mockRepo.Setup(repo => repo.CreateUserAsync(createUserRequest.Email))
                    .ReturnsAsync(expectedUser);

            // Act
            var result = await userService.CreateUserAsync(createUserRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.Id);
            Assert.Equal(expectedUser.Email, result.Email);
            Assert.Equal(expectedUser.Balance, result.Balance);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnError_WhenRepositoryThrowsException()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var userService = new UserService(mockRepo.Object, mockLogger.Object);
            var createUserRequest = new CreateUserRequest { Email = "test@example.com" };

            mockRepo.Setup(repo => repo.CreateUserAsync(createUserRequest.Email))
                    .ThrowsAsync(new Exception("Repository error"));

            // Act
            var result = await userService.CreateUserAsync(createUserRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Repository error", result.Error);
            Assert.Null(result.Id);
            Assert.Null(result.Email);
            Assert.Null(result.Balance);
        }

        [Fact]
        public async Task DepositAsync_ShouldReturnResponseDTO_WhenDepositIsSuccessful()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var userService = new UserService(mockRepo.Object, mockLogger.Object);
            var depositRequest = new DepositRequest { Amount = 100 };
            var userId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.DepositAsync(userId, It.IsAny<User>()))
                    .ReturnsAsync(100); // Предположим, что баланс после депозита равен 100

            // Act
            var result = await userService.DepositAsync(depositRequest, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.Balance);
            Assert.Equal(userId, result.Id);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task DepositAsync_ShouldReturnError_WhenRepositoryThrowsException()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var userService = new UserService(mockRepo.Object, mockLogger.Object);
            var depositRequest = new DepositRequest { Amount = 100 };
            var userId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.DepositAsync(userId, It.IsAny<User>()))
                    .ThrowsAsync(new Exception("Repository error"));

            // Act
            var result = await userService.DepositAsync(depositRequest, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Repository error", result.Error);
            Assert.Null(result.Balance);
            Assert.Null(result.Id);
        }

        [Fact]
        public async Task GetBalanceAsync_ShouldReturnResponseDTO_WhenBalanceIsRetrieved()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var userService = new UserService(mockRepo.Object, mockLogger.Object);
            var userId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetBalanceAsync(userId))
                    .ReturnsAsync(500); // Предположим, что баланс равен 500

            // Act
            var result = await userService.GetBalanceAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.Balance);
            Assert.Equal(userId, result.Id);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task GetBalanceAsync_ShouldReturnError_WhenRepositoryThrowsException()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var userService = new UserService(mockRepo.Object, mockLogger.Object);
            var userId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetBalanceAsync(userId))
                    .ThrowsAsync(new Exception("Repository error"));

            // Act
            var result = await userService.GetBalanceAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Repository error", result.Error);
            Assert.Null(result.Balance);
            Assert.Null(result.Id);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldReturnResponseDTO_WhenWithdrawalIsSuccessful()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var userService = new UserService(mockRepo.Object, mockLogger.Object);
            var withdrawRequest = new WithdrawRequest { Amount = 50 };
            var userId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.WithdrawAsync(userId, It.IsAny<User>()))
                    .ReturnsAsync(450); // Предположим, что баланс после вывода равен 450

            // Act
            var result = await userService.WithdrawAsync(withdrawRequest, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(450, result.Balance);
            Assert.Equal(userId, result.Id);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldReturnError_WhenRepositoryThrowsException()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var userService = new UserService(mockRepo.Object, mockLogger.Object);
            var withdrawRequest = new WithdrawRequest { Amount = 50 };
            var userId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.WithdrawAsync(userId, It.IsAny<User>()))
                    .ThrowsAsync(new Exception("Repository error"));

            // Act
            var result = await userService.WithdrawAsync(withdrawRequest, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Repository error", result.Error);
            Assert.Null(result.Balance);
            Assert.Null(result.Id);
        }


    }
}
