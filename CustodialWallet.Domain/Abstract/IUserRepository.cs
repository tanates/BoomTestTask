using CustodialWallet.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CustodialWallet.Domain.Abstract
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(string userEmail);

        public Task<decimal> GetBalanceAsync(Guid userId);
        public Task<decimal> DepositAsync(Guid userId, User user);
        public Task<decimal> WithdrawAsync(Guid userId, User user);

        public Task <IEnumerable<User>> GetAllUsersAsync();
    }
}
