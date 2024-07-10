using HS.Crud.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Crud.Handlers
{
    public class UpdateCommandHandler<TDto, TEntity> : ICommandHandler<UpdateCommand<TDto>>
      where TDto : class
      where TEntity : Entity
    {
        private readonly IDataReader _reader;
        private readonly IMapperFactory _mapperFactory;

        public UpdateCommandHandler(IDataReader reader, IMapperFactory mapperFactory)
        {
            _reader = reader;
            _mapperFactory = mapperFactory;
        }

        public async Task ExecuteAsync(UpdateCommand<TDto> command)
        {
            var mapper = _mapperFactory.GetUpdater<TDto, TEntity>();
            var entity = await _reader.GetAsync<TEntity>(command.Id.Guid());
            await mapper.UpdateAsync(command.Data, entity);
        }
    }
}
