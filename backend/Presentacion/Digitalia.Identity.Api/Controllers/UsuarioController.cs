using Digitalia.Identity.Aplicacion.Commands;
using Digitalia.Identity.Model;
using HS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Digitalia.Identity.Api.Controllers
{
    [ApiController]
    [Route("usuario")]
    [Authorize]
    public class UsuarioController(ICommandExecutor ce): BaseController(ce)
    {
        [HttpGet("actual")]
        public async Task<IActionResult> VerActual()
        {
            var result = await Executor.ExecuteAsync<UsuarioActualResponse, UsuarioActualCommand>(new UsuarioActualCommand());
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> ActualizarActual([FromBody] ActualizaUsuarioRequest data)
        {
            await Executor.ExecuteAsync(new ActualizaUsuarioCommand { Data = data });
            return Ok();
        }

        [HttpPost("cambio-pwd")]
        public async Task<IActionResult> CambioPwd([FromBody] CambioPwdRequest data)
        {
            await Executor.ExecuteAsync(new CambioPwdCommand { Data = data });
            return Ok();
        }
    }
}
