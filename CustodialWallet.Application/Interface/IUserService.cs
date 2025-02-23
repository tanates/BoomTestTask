using CustodialWallet.Application.DTO;
using CustodialWallet.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustodialWallet.Application.Interface
{
    public interface IUserService
    {
        public Task<ResponseDTO> CreateUserAsync(CreateUserRequest userDTO);
        public Task<ResponseDTO> GetBalanceAsync(Guid userId);
        public Task<ResponseDTO> DepositAsync(DepositRequest userDTO , Guid userId);
        public Task<ResponseDTO> WithdrawAsync(WithdrawRequest userDTO , Guid userId);
        public Task<ResponseDTO> GetAllUserAsync();
    }
}
