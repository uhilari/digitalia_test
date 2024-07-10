using HS.Crud.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Crud.Handlers
{
    public class DeleteCommandHandler<TDto, TEntity> : ICommandHandler<DeleteCommand<TDto>>
      where TDto : class
      where TEntity : Entity
    {
        private readonly IDataReader _reader;

        public DeleteCommandHandler(IDataReader reader)
        {
            _reader = reader.NoEsNull("reader");
        }

        public void Execute(DeleteCommand<TDto> command)
        {
            command.NoEsNull("command");
            var entity = _reader.Get<TEntity>(command.Id.Guid());
            entity.Eliminar();
        }

        public async Task ExecuteAsync(DeleteCommand<TDto> command)
        {
            command.NoEsNull("command");
            var entity = await _reader.GetAsync<TEntity>(command.Id.Guid());
            entity.Eliminar();
        }
    }
}
