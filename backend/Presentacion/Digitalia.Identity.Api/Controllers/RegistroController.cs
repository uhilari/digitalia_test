using Digitalia.Identity.Aplicacion.Commands;
using Digitalia.Identity.Model;
using HS;
using Microsoft.AspNetCore.Mvc;

namespace Digitalia.Identity.Api.Controllers
{
    [ApiController]
    [Route("registro")]
    public class RegistroController: BaseController
    {
        public RegistroController(ICommandExecutor commandExecutor)
            : base(commandExecutor) { }

        [HttpPost("")]
        public async Task<IActionResult> PostRegistro([FromBody] RegistroRequest data)
        {
            await Executor.ExecuteAsync(new RegistroCommand { Data = data });
            return Ok();
        }
    }
}
