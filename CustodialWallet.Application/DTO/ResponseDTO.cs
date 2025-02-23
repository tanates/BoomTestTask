
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CustodialWallet.Application.DTO
{
    public class ResponseDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [SwaggerSchema("The Email of the User.")]
        public string? Email { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [SwaggerSchema("An error message, if an error occurred during the operation.")]
        public string? Error { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [Range(0.01, double.MaxValue)]
        [SwaggerSchema("The user's account balance after the operation.")]
        public decimal? Balance{ get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [SwaggerSchema("The ID of the entity affected by the operation.")]
        public Guid? Id{ get; set; }
    }
}
