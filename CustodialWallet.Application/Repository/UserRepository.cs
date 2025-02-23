using CustodialWallet.Domain;
using CustodialWallet.Domain.Abstract;
using CustodialWallet.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustodialWallet.Application.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(
            AppDbContext appDbContext ,
            ILogger<UserRepository> logger)
        {
            _logger = logger;
            _appDbContext=appDbContext;
        }

        public async Task<User> CreateUserAsync(string userEmail)
        {
            if (userEmail == null) throw new ArgumentNullException(nameof(userEmail));
            
            var userFind = await _appDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u=>u.Email==userEmail);
            
            if (userFind != null) throw new InvalidOperationException("The user is already registered ");

            var  user = new User { Email = userEmail };

            var res = await _appDbContext.Users.AddAsync(user);
            
            var resSave = await _appDbContext.SaveChangesAsync();
            
            if (resSave==0) throw new Exception("The user is not registered");
            
            return user;
        }

        public async Task<decimal> DepositAsync(Guid userId, User userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel), "User model cannot be null.");
            }

            if (userModel.Balance < 0)
            {
                throw new ArgumentException("The amount cannot be less than 0.", nameof(userModel.Balance));
            }

            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                // Находим пользователя в базе данных
                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                {
                    throw new Exception("The user is not registered.");
                }

                // Обновляем баланс
                user.Balance += userModel.Balance;

                // Сохраняем изменения в базе данных
                var saveRes = await _appDbContext.SaveChangesAsync();

                // Если изменения не сохранены, выбрасываем исключение
                if (saveRes == 0)
                {
                    throw new Exception("Error updating balance.");
                }

                // Фиксируем транзакцию
                await transaction.CommitAsync();

                // Возвращаем новый баланс
                return user.Balance;
            }
            catch (Exception ex)
            {
                // Откатываем транзакцию в случае ошибки
                await transaction.RollbackAsync();
                throw new Exception("Deposit failed: " + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var enumerableUser = await _appDbContext.Users.AsNoTracking().ToListAsync();

            return enumerableUser;
        }

        public async Task<decimal> GetBalanceAsync(Guid userId)
        {
            var user = await _appDbContext.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId==userId);
            if (user==null) throw new InvalidOperationException($"This user ({userId}) was not found");

            return user.Balance;
        }

        public async Task<decimal> WithdrawAsync(Guid userId, User userModel)
        {
            // Проверка на null
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel), "User model cannot be null.");
            }

            // Проверка на отрицательный баланс
            if (userModel.Balance < 0)
            {
                throw new ArgumentException("The withdrawal amount cannot be less than 0.", nameof(userModel.Balance));
            }

            // Начало транзакции
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                // Поиск пользователя в базе данных
                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                {
                    throw new Exception("The user is not registered.");
                }

                // Проверка на достаточность средств
                if (userModel.Balance > user.Balance)
                {
                    throw new Exception("Insufficient funds.");
                }

                // Списание средств
                user.Balance -= userModel.Balance;

                // Сохранение изменений в базе данных
                var saveRes = await _appDbContext.SaveChangesAsync();
                if (saveRes == 0)
                {
                    throw new Exception("Error updating balance.");
                }

                // Фиксация транзакции
                await transaction.CommitAsync();

                // Возврат нового баланса
                return user.Balance;
            }
            catch (Exception ex)
            {
                // Откат транзакции в случае ошибки
                await transaction.RollbackAsync();
                throw new Exception("Withdrawal failed: " + ex.Message, ex);
            }
        }
    }
}
