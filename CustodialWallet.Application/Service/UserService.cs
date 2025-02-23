using CustodialWallet.Application.DTO;
using CustodialWallet.Application.Interface;
using CustodialWallet.Domain.Abstract;
using CustodialWallet.Domain.Entity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CustodialWallet.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache; // Можно добавить кэширование позже
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<ResponseDTO> CreateUserAsync(CreateUserRequest userDTO)
        {
            _logger.LogInformation("Starting CreateUserAsync for email: {Email}", userDTO.Email);

            try
            {
                var res = await _userRepository.CreateUserAsync(userDTO.Email);
                if (res == null)
                {
                    _logger.LogError("Failed to create user with email: {Email}", userDTO.Email);
                    throw new Exception("Ups, error creating user.");
                }

                _logger.LogInformation("User created successfully: {UserId}", res.UserId);

                return new ResponseDTO { Balance = res.Balance,Email= res.Email , Id = res.UserId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateUserAsync for email: {Email}", userDTO.Email);
                return new ResponseDTO { Error = ex.Message };
            }
        }

        public async Task<ResponseDTO> DepositAsync(DepositRequest userDTO, Guid userId)
        {
            _logger.LogInformation("Starting DepositAsync for user: {UserId}, amount: {Amount}", userId, userDTO.Amount);

            try
            {
                var userUp = new User { Balance = userDTO.Amount, UserId = userId };
                var depositRes = await _userRepository.DepositAsync(userId, userUp);

                
                _logger.LogInformation("Deposit successful for user: {UserId}, new balance: {Balance}", userId, depositRes);

                return new ResponseDTO { Balance = depositRes,  Id = userId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DepositAsync for user: {UserId}", userId);
                return new ResponseDTO { Error = ex.Message };
            }
        }

        public async Task<ResponseDTO> GetAllUserAsync() // метод сделал для своего удобства , что бы постоянно в бд ради  id не лазить )
        {
            _logger.LogInformation("Starting GetAllUserAsync");

            try
            {
                var listUser = await _userRepository.GetAllUsersAsync();
                var json = JsonConvert.SerializeObject(listUser);
                _logger.LogInformation("Retrieved {Count} users", listUser.Count());

                return new ResponseDTO { Email = json };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllUserAsync");
                throw;
            }
        }

        public async Task<ResponseDTO> GetBalanceAsync(Guid userId)
        {
            _logger.LogInformation("Starting GetBalanceAsync for user: {UserId}", userId);

            try
            {
                var balance = await _userRepository.GetBalanceAsync(userId);
                

                
                _logger.LogInformation("Balance retrieved for user: {UserId}, balance: {Balance}", userId, balance);

                return new ResponseDTO { Balance = balance, Id = userId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBalanceAsync for user: {UserId}", userId);
                return new ResponseDTO { Error = ex.Message };
            }
        }

        public async Task<ResponseDTO> WithdrawAsync(WithdrawRequest userDTO, Guid userId)
        {
            _logger.LogInformation("Starting WithdrawAsync for user: {UserId}, amount: {Amount}", userId, userDTO.Amount);

            try
            {
                var user = new User { Balance = userDTO.Amount, UserId = userId };
                var balance = await _userRepository.WithdrawAsync(user.UserId, user);

             
                _logger.LogInformation("Withdrawal successful for user: {UserId}, new balance: {Balance}", userId, balance);

                return new ResponseDTO { Balance = balance, Id = userId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in WithdrawAsync for user: {UserId}", userId);
                return new ResponseDTO { Error = ex.Message };
            }
        }
    }
}
