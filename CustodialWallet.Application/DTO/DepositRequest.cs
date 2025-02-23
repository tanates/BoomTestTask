using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustodialWallet.Application.DTO
{
    public class DepositRequest
    {
        [Required]
        [Range(0.01, double.MaxValue)]
        [SwaggerSchema("The amount to deposit into the user's account.", Nullable = false)]
        public decimal Amount { get; set; }
    }
}
