using HS.Crud.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Crud.Handlers
{
    public class CreateCommandHandler<TDto, TEntity> : ICommandHandler<CreateCommand<TDto>>
      where TDto : class
      where TEntity : Entity
    {
        private readonly IDataWriter _writer;
        private readonly IMapperFactory _mapperFactory;

        public CreateCommandHandler(IDataWriter writer, IMapperFactory mapperFactory)
        {
            _writer = writer.NoEsNull("writer");
            _mapperFactory = mapperFactory;
        }

        public async Task ExecuteAsync(CreateCommand<TDto> command)
        {
            var mapper = _mapperFactory.GetMaker<TDto, TEntity>();
            command.NoEsNull("command");
            await _writer.AddAsync(await mapper.MakeAsync(command.Data));
        }
    }
}
