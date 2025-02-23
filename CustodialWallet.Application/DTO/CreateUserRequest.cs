using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustodialWallet.Application.DTO
{
    public class CreateUserRequest
    {
        [Required]
        [EmailAddress]
        [SwaggerSchema("The email address of the new user.", Nullable = false)]
        public string Email { get; set; }
    }
}
