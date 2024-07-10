using HS.Crud.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HS
{
    public class CrudController<TDto> : BaseController
      where TDto : class
    {
        public CrudController(ICommandExecutor executor)
            : base(executor)
        {
        }

        [Route("{id}"), HttpGet]
        [SwaggerResponse(200, "Success", typeof(object))]
        [SwaggerResponse(400, "Error de request", typeof(ApiResponse<IEnumerable<Error>>))]
        public async Task<IActionResult> Obtener([FromRoute] ReadCommand<TDto> command)
        {
            var resp = await Executor.ExecuteAsync<TDto, ReadCommand<TDto>>(command);
            return Ok(resp);
        }

        [Route(""), HttpPost]
        [SwaggerResponse(200, "Success", typeof(object))]
        [SwaggerResponse(400, "Error de request", typeof(ApiResponse<IEnumerable<Error>>))]
        public async Task<IActionResult> Registrar([FromBody] TDto dto)
        {
            await Executor.ExecuteAsync(new CreateCommand<TDto> { Data = dto });
            return Ok();
        }

        [Route("{id}"), HttpPut]
        [SwaggerResponse(200, "Success", typeof(object))]
        [SwaggerResponse(400, "Error de request", typeof(ApiResponse<IEnumerable<Error>>))]
        public async Task<IActionResult> Actualizar(string id, [FromBody] TDto dto)
        {
            await Executor.ExecuteAsync(new UpdateCommand<TDto>
            {
                Id = id,
                Data = dto
            });
            return Ok();
        }

        [Route("{id}"), HttpDelete]
        [SwaggerResponse(200, "Success", typeof(object))]
        [SwaggerResponse(400, "Error de request", typeof(ApiResponse<IEnumerable<Error>>))]
        public async Task<IActionResult> Eliminar([FromRoute] DeleteCommand<TDto> command)
        {
            await Executor.ExecuteAsync(command);
            return Ok();
        }
    }
}
