using Digitalia.Identity.Aplicacion.Commands;
using Digitalia.Identity.Model;
using HS;
using Microsoft.AspNetCore.Mvc;

namespace Digitalia.Identity.Api.Controllers
{
    [ApiController]
    [Route("token")]
    public class TokenController(ICommandExecutor ce): BaseController(ce)
    {
        [HttpPost("")]
        public async Task<IActionResult> CreateToken([FromBody] CreateTokenRequest data)
        {
            var resultado = await Executor.ExecuteAsync<CreateTokenResponse, CreateTokenCommand>(new CreateTokenCommand { Data = data });
            return Ok(resultado);
        }
    }
}
