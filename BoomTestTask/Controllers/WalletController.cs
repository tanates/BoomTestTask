using CustodialWallet.Application.DTO;
using CustodialWallet.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace CustodialWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IUserService _userService;

        public WalletController(IUserService userService)
        {
            _userService=userService;
        }


        [HttpGet("user/all")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var listUser = await _userService.GetAllUserAsync();
                return Ok(listUser);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet("{userId}/balance")]
        public async Task<IActionResult> GetBalance(Guid userId)
        {
            var res = await _userService.GetBalanceAsync(userId);
            return Ok(res);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var res = await _userService.CreateUserAsync(request);
            return Ok(res);
        }

        [HttpPut("{userId}/deposit")]
        public async Task<IActionResult> Deposit(Guid userId, [FromBody] DepositRequest request)
        {
            try
            {
                var res = await _userService.DepositAsync(request, userId);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }

        [HttpPut("{userId}/withdraw")]
        public async Task<IActionResult> Withdraw(Guid userId, [FromBody] WithdrawRequest request)
        {
            try
            {
                var res = await _userService.WithdrawAsync(request, userId);
                return Ok(res);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
