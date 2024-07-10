using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Pagination
{
    public class PagedListCommandHandler<TDto, TEntity> : ICommandHandler<PagedListCommand<TDto>, PagedList<TDto>>
      where TDto : class
      where TEntity : Entity
    {
        private readonly IPagedQuery<TEntity> _query;
        private readonly IMapperFactory _mapperFactory;

        public PagedListCommandHandler(IPagedQuery<TEntity> query, IMapperFactory mapperFactory)
        {
            _query = query.NoEsNull(nameof(query));
            _mapperFactory = mapperFactory;
        }

        //public PagedList<TDto> Execute(PagedListCommand<TDto> command)
        //{
        //    var mapper = _mapperFactory.GetMaker<TEntity, TDto>();
        //    _query.Pagina = command.Pagina;
        //    _query.Numero = command.Limite;
        //    var pg = _query.Execute();
        //    return new PagedList<TDto>
        //    {
        //        Total = pg.Total,
        //        Items = pg.Items.Select(i => mapper.MakeAsync(i))
        //                    .Select(t => t.Result)
        //                    .ToArray()
        //    };
        //}

        public async Task<PagedList<TDto>> ExecuteAsync(PagedListCommand<TDto> command)
        {
            var mapper = _mapperFactory.GetMaker<TEntity, TDto>();
            _query.Pagina = command.Pagina;
            _query.Numero = command.Limite;
            var pg = await _query.ExecuteAsync();
            return new PagedList<TDto>
            {
                Total = pg.Total,
                Items = pg.Items.Select(i => mapper.MakeAsync(i))
                            .Select(t => t.Result)
                            .ToArray()
            };
        }
    }

    public class PagedListCommandHandler<TDto> : ICommandHandler<PagedListCommand<TDto>, PagedList<TDto>>
      where TDto : class
    {
        private IPagedQuery<TDto> _query;

        public PagedListCommandHandler(IPagedQuery<TDto> query)
        {
            _query = query;
        }

        //public PagedList<TDto> Execute(PagedListCommand<TDto> command)
        //{
        //    _query.Pagina = command.Pagina;
        //    _query.Numero = command.Limite;
        //    return _query.Execute();
        //}

        public async Task<PagedList<TDto>> ExecuteAsync(PagedListCommand<TDto> command)
        {
            _query.Pagina = command.Pagina;
            _query.Numero = command.Limite;
            return await _query.ExecuteAsync();
        }
    }
}
