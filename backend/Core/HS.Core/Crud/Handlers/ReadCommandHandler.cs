using HS.Crud.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Crud.Handlers
{
    public class ReadCommandHandler<TDto, TEntity> : ICommandHandler<ReadCommand<TDto>, TDto>
      where TDto : class
      where TEntity : Entity
    {
        private readonly IDataReader _reader;
        private readonly IMapperFactory _mapperFactory;

        public ReadCommandHandler(IDataReader reader, IMapperFactory mapperFactory)
        {
            _reader = reader.NoEsNull("reader");
            _mapperFactory = mapperFactory;
        }

        public async Task<TDto> ExecuteAsync(ReadCommand<TDto> command)
        {
            var mapper = _mapperFactory.GetMaker<TEntity, TDto>();
            command.NoEsNull("command");
            return await mapper.MakeAsync(await _reader.GetAsync<TEntity>(command.Id.Guid()));
        }
    }
}
