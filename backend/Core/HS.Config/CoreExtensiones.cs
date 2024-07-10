using HS.Crud.Commands;
using HS.Crud.Handlers;
using HS.Pagination;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace HS
{
    public static class CoreExtensiones
    {
        public static IServiceCollection AddCreateCommand<TDto, TEntity>(this IServiceCollection container, Action<CommandOption<CreateCommand<TDto>>> config = null)
          where TDto : class
          where TEntity : Entity
        {
            container.AddTransient<ICommandHandler<CreateCommand<TDto>>, CreateCommandHandler<TDto, TEntity>>()
                .Decorate<ICommandHandler<CreateCommand<TDto>>, TransactionDecorator<CreateCommand<TDto>>>()
                .Decorate<ICommandHandler<CreateCommand<TDto>>, CommandValidationDecorator<CreateCommand<TDto>>>()
                .Decorate<ICommandHandler<CreateCommand<TDto>>, SessionDecorator<CreateCommand<TDto>>>();
            config?.Invoke(new CommandOption<CreateCommand<TDto>>(container));
            return container;
        }

        public static IServiceCollection AddCreateCommandHandler<TDto, THandler>(this IServiceCollection container, Action<CommandOption<CreateCommand<TDto>>> config = null)
          where TDto : class
          where THandler : class, ICommandHandler<CreateCommand<TDto>>
        {
            container.AddTransient<ICommandHandler<CreateCommand<TDto>>, THandler>()
                .Decorate<ICommandHandler<CreateCommand<TDto>>, TransactionDecorator<CreateCommand<TDto>>>()
                .Decorate<ICommandHandler<CreateCommand<TDto>>, CommandValidationDecorator<CreateCommand<TDto>>>()
                .Decorate<ICommandHandler<CreateCommand<TDto>>, SessionDecorator<CreateCommand<TDto>>>();
            config?.Invoke(new CommandOption<CreateCommand<TDto>>(container));
            return container;
        }

        public static IServiceCollection AddReadCommand<TDto, TEntity>(this IServiceCollection container, Action<CommandOption<ReadCommand<TDto>>> config = null)
          where TDto : class
          where TEntity : Entity
        {
            container.AddTransient<ICommandHandler<ReadCommand<TDto>, TDto>, ReadCommandHandler<TDto, TEntity>>()
                .Decorate<ICommandHandler<ReadCommand<TDto>, TDto>, CommandValidationDecorator<ReadCommand<TDto>, TDto>>()
                .Decorate<ICommandHandler<ReadCommand<TDto>, TDto>, SessionDecorator<ReadCommand<TDto>, TDto>>();
            config?.Invoke(new CommandOption<ReadCommand<TDto>>(container));
            return container;
        }

        public static IServiceCollection AddReadCommandHandler<TDto, THandler>(this IServiceCollection container, Action<CommandOption<ReadCommand<TDto>>> config = null)
          where TDto : class
          where THandler : class, ICommandHandler<ReadCommand<TDto>, TDto>
        {
            container.AddTransient<ICommandHandler<ReadCommand<TDto>, TDto>, THandler>()
                .Decorate<ICommandHandler<ReadCommand<TDto>, TDto>, CommandValidationDecorator<ReadCommand<TDto>, TDto>>()
                .Decorate<ICommandHandler<ReadCommand<TDto>, TDto>, SessionDecorator<ReadCommand<TDto>, TDto>>();
            config?.Invoke(new CommandOption<ReadCommand<TDto>>(container));
            return container;
        }

        public static IServiceCollection AddUpdateCommand<TDto, TEntity>(this IServiceCollection container, Action<CommandOption<UpdateCommand<TDto>>> config = null)
          where TDto : class
          where TEntity : Entity
        {
            container.AddTransient<ICommandHandler<UpdateCommand<TDto>>, UpdateCommandHandler<TDto, TEntity>>()
                .Decorate<ICommandHandler<UpdateCommand<TDto>>, TransactionDecorator<UpdateCommand<TDto>>>()
                .Decorate<ICommandHandler<UpdateCommand<TDto>>, CommandValidationDecorator<UpdateCommand<TDto>>>()
                .Decorate<ICommandHandler<UpdateCommand<TDto>>, SessionDecorator<UpdateCommand<TDto>>>();
            config?.Invoke(new CommandOption<UpdateCommand<TDto>>(container));
            return container;
        }

        public static IServiceCollection AddUpdateCommandHandler<TDto, THandler>(this IServiceCollection container, Action<CommandOption<UpdateCommand<TDto>>> config = null)
          where TDto : class
          where THandler : class, ICommandHandler<UpdateCommand<TDto>>
        {
            container.AddTransient<ICommandHandler<UpdateCommand<TDto>>, THandler>()
                .Decorate<ICommandHandler<UpdateCommand<TDto>>, TransactionDecorator<UpdateCommand<TDto>>>()
                .Decorate<ICommandHandler<UpdateCommand<TDto>>, CommandValidationDecorator<UpdateCommand<TDto>>>()
                .Decorate<ICommandHandler<UpdateCommand<TDto>>, SessionDecorator<UpdateCommand<TDto>>>();
            config?.Invoke(new CommandOption<UpdateCommand<TDto>>(container));
            return container;
        }

        public static IServiceCollection AddDeleteCommand<TDto, TEntity>(this IServiceCollection container, Action<CommandOption<DeleteCommand<TDto>>> config = null)
          where TDto : class
          where TEntity : Entity
        {
            container.AddTransient<ICommandHandler<DeleteCommand<TDto>>, DeleteCommandHandler<TDto, TEntity>>()
                .Decorate<ICommandHandler<DeleteCommand<TDto>>, TransactionDecorator<DeleteCommand<TDto>>>()
                .Decorate<ICommandHandler<DeleteCommand<TDto>>, CommandValidationDecorator<DeleteCommand<TDto>>>()
                .Decorate<ICommandHandler<DeleteCommand<TDto>>, SessionDecorator<DeleteCommand<TDto>>>();
            config?.Invoke(new CommandOption<DeleteCommand<TDto>>(container));
            return container;
        }

        public static IServiceCollection AddDeleteCommandHandler<TDto, THandler>(this IServiceCollection container, Action<CommandOption<DeleteCommand<TDto>>> config = null)
          where TDto : class
          where THandler : class, ICommandHandler<DeleteCommand<TDto>>
        {
            container.AddTransient<ICommandHandler<DeleteCommand<TDto>>, THandler>()
                .Decorate<ICommandHandler<DeleteCommand<TDto>>, TransactionDecorator<DeleteCommand<TDto>>>()
                .Decorate<ICommandHandler<DeleteCommand<TDto>>, CommandValidationDecorator<DeleteCommand<TDto>>>()
                .Decorate<ICommandHandler<DeleteCommand<TDto>>, SessionDecorator<DeleteCommand<TDto>>>();
            config?.Invoke(new CommandOption<DeleteCommand<TDto>>(container));
            return container;
        }

        public static IServiceCollection AddCrudCommands<TDto, TEntity>(this IServiceCollection container)
          where TDto : class
          where TEntity : Entity
        {
            container
              .AddCreateCommand<TDto, TEntity>()
              .AddReadCommand<TDto, TEntity>()
              .AddUpdateCommand<TDto, TEntity>()
              .AddDeleteCommand<TDto, TEntity>();

            return container;
        }

        public static IServiceCollection AddCrudCommandHandlers<TDto, THandler>(this IServiceCollection container)
          where TDto : class
          where THandler : class,
            ICommandHandler<CreateCommand<TDto>>,
            ICommandHandler<ReadCommand<TDto>, TDto>,
            ICommandHandler<UpdateCommand<TDto>>,
            ICommandHandler<DeleteCommand<TDto>>
        {
            container
              .AddCreateCommandHandler<TDto, THandler>()
              .AddReadCommandHandler<TDto, THandler>()
              .AddUpdateCommandHandler<TDto, THandler>()
              .AddDeleteCommandHandler<TDto, THandler>();

            return container;
        }

        public static IServiceCollection AddPagedListCommand<TDto, TEntity>(this IServiceCollection container, Action<CommandOption<PagedListCommand<TDto>>> config = null)
            where TDto : class
            where TEntity : Entity
        {
            container.AddTransient<ICommandHandler<PagedListCommand<TDto>, PagedList<TDto>>, PagedListCommandHandler<TDto, TEntity>>()
                .Decorate<ICommandHandler<PagedListCommand<TDto>, PagedList<TDto>>, CommandValidationDecorator<PagedListCommand<TDto>, PagedList<TDto>>>()
                .Decorate<ICommandHandler<PagedListCommand<TDto>, PagedList<TDto>>, SessionDecorator<PagedListCommand<TDto>, PagedList<TDto>>>();
            config?.Invoke(new CommandOption<PagedListCommand<TDto>>(container));
            return container;
        }

        public static IServiceCollection AddPagedListCommand<TDto>(this IServiceCollection container, Action<CommandOption<PagedListCommand<TDto>>> config = null)
            where TDto : class
        {
            container.AddTransient<ICommandHandler<PagedListCommand<TDto>, PagedList<TDto>>, PagedListCommandHandler<TDto>>()
                .Decorate<ICommandHandler<PagedListCommand<TDto>, PagedList<TDto>>, CommandValidationDecorator<PagedListCommand<TDto>, PagedList<TDto>>>()
                .Decorate<ICommandHandler<PagedListCommand<TDto>, PagedList<TDto>>, SessionDecorator<PagedListCommand<TDto>, PagedList<TDto>>>();
            config?.Invoke(new CommandOption<PagedListCommand<TDto>>(container));
            return container;
        }

        public static IServiceCollection AddCommand<TCommand, THandler>(this IServiceCollection container, Action<CommandOption<TCommand>> config = null)
            where TCommand : Command
            where THandler : class, ICommandHandler<TCommand>
        {
            container.AddTransient<ICommandHandler<TCommand>, THandler>()
                .Decorate<ICommandHandler<TCommand>, TransactionDecorator<TCommand>>()
                .Decorate<ICommandHandler<TCommand>, CommandValidationDecorator<TCommand>>()
                .Decorate<ICommandHandler<TCommand>, SessionDecorator<TCommand>>();
            config?.Invoke(new CommandOption<TCommand>(container));
            return container;
        }

        public static IServiceCollection AddCommand<TCommand, THandler, TResult>(this IServiceCollection container, Action<CommandOption<TCommand>> config = null)
          where TCommand : Command<TResult>
          where THandler : class, ICommandHandler<TCommand, TResult>
        {
            container.AddTransient<ICommandHandler<TCommand, TResult>, THandler>()
                .Decorate<ICommandHandler<TCommand, TResult>, TransactionDecorator<TCommand, TResult>>()
                .Decorate<ICommandHandler<TCommand, TResult>, CommandValidationDecorator<TCommand, TResult>>()
                .Decorate<ICommandHandler<TCommand, TResult>, SessionDecorator<TCommand, TResult>>();
            config?.Invoke(new CommandOption<TCommand>(container));
            return container;
        }

        public static IServiceCollection AddCommand(this IServiceCollection container, Type tCommand, Type tHandler)
        {
            var tipoCommandHandler = typeof(ICommandHandler<>).MakeGenericType(tCommand);
            var transactionDecorator = typeof(TransactionDecorator<>).MakeGenericType(tCommand);
            var validatorDecorator = typeof(CommandValidationDecorator<>).MakeGenericType(tCommand);
            var sessionDecorator = typeof(SessionDecorator<>).MakeGenericType(tCommand);
            container.AddTransient(tipoCommandHandler, tHandler)
                .Decorate(tipoCommandHandler, transactionDecorator)
                .Decorate(tipoCommandHandler, validatorDecorator)
                .Decorate(tipoCommandHandler, sessionDecorator);
            return container;
        }

        public static IServiceCollection AddMaker<TSrc, TDst, TMaker>(this IServiceCollection container)
            where TSrc : class
            where TDst : class
            where TMaker : class, IMaker<TSrc, TDst>
        {
            container.AddTransient<IMaker<TSrc, TDst>, TMaker>();
            return container;
        }

        public static IServiceCollection AddUpdater<TSrc, TDst, TUpdater>(this IServiceCollection container)
            where TSrc : class
            where TDst : class
            where TUpdater : class, IUpdater<TSrc, TDst>
        {
            container.AddTransient<IUpdater<TSrc, TDst>, TUpdater>();
            return container;
        }

        public static IServiceCollection AddBothMaker<TSrc, TDst, TMaker>(this IServiceCollection container)
            where TSrc : class
            where TDst : class
            where TMaker : class, IMaker<TSrc, TDst>, IMaker<TDst, TSrc>
        {
            container.AddMaker<TSrc, TDst, TMaker>();
            container.AddMaker<TDst, TSrc, TMaker>();
            return container;
        }

        public static IServiceCollection AddCrudMapper<TSrc, TDst, TMapper>(this IServiceCollection container)
            where TSrc : class
            where TDst : class
            where TMapper : class, IMaker<TSrc, TDst>, IMaker<TDst, TSrc>, IUpdater<TSrc, TDst>
        {
            container.AddMaker<TSrc, TDst, TMapper>();
            container.AddMaker<TDst, TSrc, TMapper>();
            container.AddUpdater<TSrc, TDst, TMapper>();
            return container;
        }

        public static IServiceCollection AddCore(this IServiceCollection container)
        {
            container.AddHttpContextAccessor();
            container.AddSingleton<ICommandExecutor, CommandExecutor>();
            container.AddScoped<IMapperFactory, MapperFactory>();
            container.AddTransient<IDataReader, DataReader>();
            container.AddTransient<IDataWriter, DataWriter>();
            container.AddTransient(typeof(IPagedQuery<>), typeof(PagedQuery<>));
            container.AddTransient<IEntorno, WebApiEntorno>();
            return container;
        }

        public static IServiceCollection AddValidators(this IServiceCollection container, Assembly ensamblado)
        {
            var tipos = ensamblado.GetTypes().Where(c => c.Name.EndsWith("Validator"));
            foreach (var tipo in tipos)
            {
                var interfaces = tipo.GetInterfaces();
                foreach (var i in interfaces)
                {
                    container.AddTransient(i, tipo);
                }
            }
            return container;
        }
    }
}
