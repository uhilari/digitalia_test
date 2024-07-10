using HS.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HS
{
    public class MapperFactory_Old: IMapperFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<Type, Type> _mappers = new Dictionary<Type, Type>();

        public MapperFactory_Old(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private Type CreateMakerClass(Type typeSrc, Type typeDst)
        {
            var assemblyName = new AssemblyName();
            assemblyName.Name = $"Maker{typeSrc.Name}{typeDst.Name}Asm";
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var assemblyModule = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = assemblyModule.DefineType($"Maker{typeSrc.Name}{typeDst.Name}", TypeAttributes.Class | TypeAttributes.Public, typeof(Mapper));
            MakeConstructor(typeBuilder);
            var typeInterface = typeof(IMaker<,>);
            var typeMaker = typeInterface.MakeGenericType(typeSrc, typeDst);
            typeBuilder.AddInterfaceImplementation(typeMaker);

            var methodInternalBuilder = typeBuilder.DefineMethod("InternalMaker", MethodAttributes.Family | MethodAttributes.Virtual, typeof(object), new Type[] { typeof(object) });
            MakerInternalMethod(methodInternalBuilder, typeSrc, typeDst);

            var methodBuilder = typeBuilder.DefineMethod("Make", MethodAttributes.Public | MethodAttributes.Virtual, typeDst, new Type[] { typeSrc }); 
            MakerMethod(methodBuilder, typeSrc, typeDst);

            var returnType = typeof(Task<>).MakeGenericType(typeDst);
            var methodAsyncBuilder = typeBuilder.DefineMethod("MakeAsync", MethodAttributes.Public | MethodAttributes.Virtual, returnType, new Type[] { typeSrc });
            MakerAsyncMethod(methodAsyncBuilder, typeSrc, typeDst);

            typeBuilder.DefineMethodOverride(methodInternalBuilder, typeof(Mapper).GetMethod("InternalMaker", BindingFlags.NonPublic | BindingFlags.Instance));
            return typeBuilder.CreateType();
        }

        private void MakeConstructor(TypeBuilder typeBuilder)
        {
            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(IServiceProvider) });
            var ilGenerator = ctor.GetILGenerator();

            var baseCtor = typeof(Mapper).GetConstructor(new Type[] { typeof(IServiceProvider) });
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Call, baseCtor);

            ilGenerator.Emit(OpCodes.Ret);
        }

        private void CopyProvider(ILGenerator ilGenerator, LocalBuilder localDst, Type typeDst)
        {
            if (typeDst.EsHijo(typeof(RootEntity)))
            {
                var propMapperProvider = typeof(Mapper).GetProperty("Provider", BindingFlags.NonPublic | BindingFlags.Instance);
                var getMapperProvider = propMapperProvider.GetGetMethod(true);
                var setEntityProvider = typeof(HS.Extensiones).GetMethod("SetProvider");

                var localProvider = ilGenerator.DeclareLocal(typeof(IServiceProvider));
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Callvirt, getMapperProvider);
                ilGenerator.Emit(OpCodes.Stloc, localProvider);

                ilGenerator.Emit(OpCodes.Ldloc, localDst);
                ilGenerator.Emit(OpCodes.Ldloc, localProvider);
                ilGenerator.Emit(OpCodes.Call, setEntityProvider);
            }
        }

        private void MakerMethod(MethodBuilder methodBuilder, Type typeSrc, Type typeDst) {
            var ilGenerator = methodBuilder.GetILGenerator();

            var methodInternal = typeof(Mapper).GetMethod("InternalMaker", BindingFlags.NonPublic | BindingFlags.Instance);
            var localDst = ilGenerator.DeclareLocal(typeDst);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Callvirt, methodInternal);
            ilGenerator.Emit(OpCodes.Stloc, localDst);

            ilGenerator.Emit(OpCodes.Ldloc, localDst);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private void MakerAsyncMethod(MethodBuilder methodBuilder, Type typeSrc, Type typeDst)
        {
            var ilGenerator = methodBuilder.GetILGenerator();

            var methodInternalGeneric = typeof(Mapper).GetMethod("InternalMakerAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var methodInternal = methodInternalGeneric.MakeGenericMethod(typeSrc, typeDst);
            var localDst = ilGenerator.DeclareLocal(typeDst);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Callvirt, methodInternal);
            ilGenerator.Emit(OpCodes.Stloc, localDst);

            ilGenerator.Emit(OpCodes.Ldloc, localDst);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private void MakerInternalMethod(MethodBuilder methodBuilder, Type typeSrc, Type typeDst)
        {
            var ilGenerator = methodBuilder.GetILGenerator();

            var dstConstructor = typeDst.GetConstructor(new Type[0]);
            var localDst = ilGenerator.DeclareLocal(typeDst);
            ilGenerator.Emit(OpCodes.Newobj, dstConstructor);
            ilGenerator.Emit(OpCodes.Stloc, localDst);

            CopyProvider(ilGenerator, localDst, typeDst);
            CopyProperties(ilGenerator, localDst, typeSrc, typeDst, CopyAction.Create);
            EntornoProperties(ilGenerator, localDst, typeDst);

            ilGenerator.Emit(OpCodes.Ldloc, localDst);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private void EntornoProperties(ILGenerator ilGenerator, LocalBuilder localDst, Type typeDst)
        {
            var idUsrProp = typeDst.GetProperty("IdUsuario");
            if (idUsrProp != null)
            {
                var setMethod = idUsrProp.GetSetMethod();
                if (setMethod != null)
                {
                    var localGuid = ilGenerator.DeclareLocal(typeof(Guid));
                    var methodIdUsr = typeof(Mapper).GetMethod("GetIdUsuario", BindingFlags.NonPublic | BindingFlags.Instance);
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.EmitCall(OpCodes.Callvirt, methodIdUsr, new Type[0]);
                    ilGenerator.Emit(OpCodes.Stloc, localGuid);

                    ilGenerator.Emit(OpCodes.Ldloc, localDst);
                    ilGenerator.Emit(OpCodes.Ldloc, localGuid);
                    ilGenerator.EmitCall(OpCodes.Call, setMethod, new Type[0]);
                }
            }
        }

        private void CopyProperties(ILGenerator ilGenerator, LocalBuilder localDst, Type typeSrc, Type typeDst, CopyAction action)
        { 
            var srcProperties = typeSrc.GetProperties();
            foreach (var srcProp in srcProperties)
            {
                var dstProp = typeDst.GetProperty(srcProp.Name);
                if (dstProp != null)
                {
                    var getMethod = srcProp.GetGetMethod();
                    var setMethod = dstProp.GetSetMethod();
                    if (getMethod == null || setMethod == null)
                        continue;
                    if (action == CopyAction.Update && srcProp.GetCustomAttribute<OnlyCreateAttribute>() != null)
                        continue;
                    else if (srcProp.PropertyType == typeof(Guid) && dstProp.PropertyType == typeof(string))
                    {
                        var localGuid = ilGenerator.DeclareLocal(srcProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.EmitCall(OpCodes.Call, getMethod, new Type[0]);
                        ilGenerator.Emit(OpCodes.Stloc, localGuid);

                        var localString = ilGenerator.DeclareLocal(dstProp.PropertyType);
                        var methodCadena = typeof(HS.Extensiones).GetMethod("Cadena", new Type[] { srcProp.PropertyType });
                        ilGenerator.Emit(OpCodes.Ldloc, localGuid);
                        ilGenerator.EmitCall(OpCodes.Call, methodCadena, new Type[0]);
                        ilGenerator.Emit(OpCodes.Stloc, localString);

                        ilGenerator.Emit(OpCodes.Ldloc, localDst);
                        ilGenerator.Emit(OpCodes.Ldloc, localString);
                        ilGenerator.EmitCall(OpCodes.Call, setMethod, new Type[0]);
                    }
                    else if (srcProp.PropertyType.IsEnum && dstProp.PropertyType == typeof(string))
                    {
                        var localEnum = ilGenerator.DeclareLocal(srcProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.EmitCall(OpCodes.Call, getMethod, new Type[0]);
                        ilGenerator.Emit(OpCodes.Stloc, localEnum);
                        
                        var localString = ilGenerator.DeclareLocal(dstProp.PropertyType);
                        var methodGeneric = typeof(Mapper).GetMethod("GetEnumString", BindingFlags.NonPublic | BindingFlags.Instance);
                        var method = methodGeneric.MakeGenericMethod(srcProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Ldloc, localEnum);
                        ilGenerator.EmitCall(OpCodes.Callvirt, method, new Type[0]);
                        ilGenerator.Emit(OpCodes.Stloc, localString);

                        ilGenerator.Emit(OpCodes.Ldloc, localDst);
                        ilGenerator.Emit(OpCodes.Ldloc, localString);
                        ilGenerator.EmitCall(OpCodes.Call, setMethod, new Type[0]);
                    }
                    else if (srcProp.PropertyType == typeof(string) && dstProp.PropertyType.IsEnum)
                    {
                        var localString = ilGenerator.DeclareLocal(srcProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.EmitCall(OpCodes.Call, getMethod, new Type[0]);
                        ilGenerator.Emit(OpCodes.Stloc, localString);
                        
                        var localEnum = ilGenerator.DeclareLocal(dstProp.PropertyType);
                        var methodGeneric = typeof(Mapper).GetMethod("GetStringEnum", BindingFlags.NonPublic | BindingFlags.Instance);
                        var method = methodGeneric.MakeGenericMethod(dstProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Ldloc, localString);
                        ilGenerator.EmitCall(OpCodes.Callvirt, method, new Type[0]);
                        ilGenerator.Emit(OpCodes.Stloc, localEnum);

                        ilGenerator.Emit(OpCodes.Ldloc, localDst);
                        ilGenerator.Emit(OpCodes.Ldloc, localEnum);
                        ilGenerator.EmitCall(OpCodes.Call, setMethod, new Type[0]);
                    }
                    else if (srcProp.PropertyType == typeof(string) && dstProp.PropertyType.EsHijo(typeof(Entity)))
                    {
                        var localString2 = ilGenerator.DeclareLocal(srcProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.Emit(OpCodes.Callvirt, getMethod);
                        ilGenerator.Emit(OpCodes.Stloc, localString2);

                        var localGuid = ilGenerator.DeclareLocal(typeof(Guid));
                        var methodGuid = typeof(HS.Extensiones).GetMethod("Guid", new Type[] { srcProp.PropertyType });
                        ilGenerator.Emit(OpCodes.Ldloc, localString2);
                        ilGenerator.EmitCall(OpCodes.Call, methodGuid, new Type[0]);
                        ilGenerator.Emit(OpCodes.Stloc, localGuid);

                        var localEntity = ilGenerator.DeclareLocal(dstProp.PropertyType);
                        var getEntityMethodGeneric = typeof(Mapper).GetMethod("Get", BindingFlags.NonPublic | BindingFlags.Instance);
                        var getEntityMethod = getEntityMethodGeneric.MakeGenericMethod(dstProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Ldloc, localGuid);
                        ilGenerator.Emit(OpCodes.Callvirt, getEntityMethod);
                        ilGenerator.Emit(OpCodes.Stloc, localEntity);

                        ilGenerator.Emit(OpCodes.Ldloc, localDst);
                        ilGenerator.Emit(OpCodes.Ldloc, localEntity);
                        ilGenerator.EmitCall(OpCodes.Call, setMethod, new Type[0]);
                    }
                    else if (srcProp.PropertyType.EsHijo(typeof(Entity)) && dstProp.PropertyType == typeof(string))
                    {
                        var localEntity = ilGenerator.DeclareLocal(srcProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.EmitCall(OpCodes.Call, getMethod, new Type[0]);
                        ilGenerator.Emit(OpCodes.Stloc, localEntity);

                        var localString = ilGenerator.DeclareLocal(typeof(string));
                        var endNotNull = ilGenerator.DefineLabel();

                        ilGenerator.Emit(OpCodes.Ldloc, localEntity);
                        ilGenerator.Emit(OpCodes.Ldnull);
                        ilGenerator.Emit(OpCodes.Ceq);
                        ilGenerator.Emit(OpCodes.Brtrue, endNotNull);

                        var localGuid = ilGenerator.DeclareLocal(typeof(Guid));
                        var propId = typeof(Entity).GetProperty("Id");
                        var methodId = propId.GetGetMethod();
                        ilGenerator.Emit(OpCodes.Ldloc, localEntity);
                        ilGenerator.Emit(OpCodes.Callvirt, methodId);
                        ilGenerator.Emit(OpCodes.Stloc, localGuid);

                        var methodCadena = typeof(HS.Extensiones).GetMethod("Cadena", new Type[] { typeof(Guid) });
                        ilGenerator.Emit(OpCodes.Ldloc, localGuid);
                        ilGenerator.EmitCall(OpCodes.Call, methodCadena, new Type[0]);
                        ilGenerator.Emit(OpCodes.Stloc, localString);

                        ilGenerator.MarkLabel(endNotNull);
                        ilGenerator.Emit(OpCodes.Ldloc, localDst);
                        ilGenerator.Emit(OpCodes.Ldloc, localString);
                        ilGenerator.EmitCall(OpCodes.Call, setMethod, new Type[0]);
                    }
                    else if (srcProp.PropertyType.IsEnumerable() && dstProp.PropertyType.EsLista())
                    {
                        var localLista = ilGenerator.DeclareLocal(srcProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.Emit(OpCodes.Callvirt, getMethod);
                        ilGenerator.Emit(OpCodes.Stloc, localLista);

                        var localEnumerable = ilGenerator.DeclareLocal(dstProp.PropertyType);
                        var getEnumerableGeneric = typeof(Mapper).GetMethod("GetLista", BindingFlags.NonPublic | BindingFlags.Instance);
                        var getEnumerable = getEnumerableGeneric.MakeGenericMethod(dstProp.PropertyType.GetGenArg(), srcProp.PropertyType.GetGenArg());
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Ldloc, localLista);
                        ilGenerator.Emit(OpCodes.Callvirt, getEnumerable);
                        ilGenerator.Emit(OpCodes.Stloc, localEnumerable);

                        ilGenerator.Emit(OpCodes.Ldloc, localDst);
                        ilGenerator.Emit(OpCodes.Ldloc, localEnumerable);
                        ilGenerator.EmitCall(OpCodes.Call, setMethod, new Type[0]);
                    }
                    else if (srcProp.PropertyType.EsLista() && dstProp.PropertyType.IsEnumerable())
                    {
                        var localLista = ilGenerator.DeclareLocal(srcProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.Emit(OpCodes.Callvirt, getMethod);
                        ilGenerator.Emit(OpCodes.Stloc, localLista);

                        var localEnumerable = ilGenerator.DeclareLocal(dstProp.PropertyType);
                        var getEnumerableGeneric = typeof(Mapper).GetMethod("GetEnumerable", BindingFlags.NonPublic | BindingFlags.Instance);
                        var getEnumerable = getEnumerableGeneric.MakeGenericMethod(dstProp.PropertyType.GetGenArg(), srcProp.PropertyType.GetGenArg());
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Ldloc, localLista);
                        ilGenerator.Emit(OpCodes.Callvirt, getEnumerable);
                        ilGenerator.Emit(OpCodes.Stloc, localEnumerable);

                        ilGenerator.Emit(OpCodes.Ldloc, localDst);
                        ilGenerator.Emit(OpCodes.Ldloc, localEnumerable);
                        ilGenerator.EmitCall(OpCodes.Call, setMethod, new Type[0]);
                    }
                    else if (srcProp.PropertyType.IsAssignableClass() && dstProp.PropertyType.IsAssignableClass())
                    {
                        var localSrc = ilGenerator.DeclareLocal(srcProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.Emit(OpCodes.Callvirt, getMethod);
                        ilGenerator.Emit(OpCodes.Stloc, localSrc);

                        var localObj = ilGenerator.DeclareLocal(dstProp.PropertyType);
                        var getObjectGeneric = typeof(Mapper).GetMethod("GetObject", BindingFlags.NonPublic | BindingFlags.Instance);
                        var getObject = getObjectGeneric.MakeGenericMethod(srcProp.PropertyType, dstProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Ldloc, localSrc);
                        ilGenerator.Emit(OpCodes.Callvirt, getObject);
                        ilGenerator.Emit(OpCodes.Stloc, localObj);

                        ilGenerator.Emit(OpCodes.Ldloc, localDst);
                        ilGenerator.Emit(OpCodes.Ldloc, localObj);
                        ilGenerator.EmitCall(OpCodes.Call, setMethod, new Type[0]);
                    }
                    else if (srcProp.Name != "Id" && dstProp.Name != "Id")
                    {
                        var localValue = ilGenerator.DeclareLocal(srcProp.PropertyType);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.EmitCall(OpCodes.Call, getMethod, new Type[0]);
                        ilGenerator.Emit(OpCodes.Stloc, localValue);

                        ilGenerator.Emit(OpCodes.Ldloc, localDst);
                        ilGenerator.Emit(OpCodes.Ldloc, localValue);
                        ilGenerator.EmitCall(OpCodes.Call, setMethod, new Type[0]);
                    }
                }
            }
        }

        private Type CreateUpdaterClass(Type typeSrc, Type typeDst)
        {
            var assemblyName = new AssemblyName();
            assemblyName.Name = $"Updater{typeSrc.Name}{typeDst.Name}Asm";
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var assemblyModule = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = assemblyModule.DefineType($"Updater{typeSrc.Name}{typeDst.Name}", TypeAttributes.Public | TypeAttributes.Class, typeof(Mapper));
            MakeConstructor(typeBuilder);
            var typeInterface = typeof(IUpdater<,>);
            var typeMaker = typeInterface.MakeGenericType(typeSrc, typeDst);
            typeBuilder.AddInterfaceImplementation(typeMaker);
            var methodBuilder = typeBuilder.DefineMethod("Update", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new Type[] { typeSrc, typeDst });
            UpdaterMethod(methodBuilder, typeSrc, typeDst);
            return typeBuilder.CreateType();
        }

        private void UpdaterMethod(MethodBuilder methodBuilder, Type typeSrc, Type typeDst)
        {
            var ilGenerator = methodBuilder.GetILGenerator();

            var localDst = ilGenerator.DeclareLocal(typeDst);
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Stloc, localDst);

            CopyProperties(ilGenerator, localDst, typeSrc, typeDst, CopyAction.Update);

            ilGenerator.Emit(OpCodes.Ret);
        }

        public IMaker<TSrc, TDst> GetMaker<TSrc, TDst>()
            where TSrc: class
            where TDst: class
        {
            var typeMaker = typeof(IMaker<TSrc, TDst>);

            var service = (IMaker<TSrc, TDst>)_serviceProvider.GetService(typeMaker);
            if (service != null)
                return service;

            if (_mappers.ContainsKey(typeMaker))
                return (IMaker<TSrc, TDst>)Activator.CreateInstance(_mappers[typeMaker], _serviceProvider);

            var typeMakerClass = CreateMakerClass(typeof(TSrc), typeof(TDst));
            _mappers.Add(typeMaker, typeMakerClass);
            return (IMaker<TSrc, TDst>)Activator.CreateInstance(typeMakerClass, _serviceProvider);
        }

        public IUpdater<TSrc, TDst> GetUpdater<TSrc, TDst>()
            where TSrc : class
            where TDst : class
        {
            var typeUpdater = typeof(IUpdater<TSrc, TDst>);

            var service = (IUpdater<TSrc, TDst>)_serviceProvider.GetService(typeUpdater);
            if (service != null)
                return service;

            if (_mappers.ContainsKey(typeUpdater))
                return (IUpdater<TSrc, TDst>)Activator.CreateInstance(_mappers[typeUpdater], _serviceProvider);

            var typeUpdaterClass = CreateUpdaterClass(typeof(TSrc), typeof(TDst));
            _mappers.Add(typeUpdater, typeUpdaterClass);
            return (IUpdater<TSrc, TDst>)Activator.CreateInstance(typeUpdaterClass, _serviceProvider);
        }

        public class Mapper
        {
            private IDataReader _reader = null;
            private IMapperFactory _mapperFactory;
            private IEntorno _entorno = null;

            public Mapper(IServiceProvider provider)
            {
                Provider = provider;
                _mapperFactory = provider.GetService<IMapperFactory>();
            }

            protected virtual object InternalMaker(object source) { throw new NotImplementedException("No se ha implementado el Maker"); }

            protected virtual void InternalUpdater(object source, object destination) { throw new NotImplementedException("No se ha implementado el Updater"); }

            protected Task<TDst> InternalMakerAsync<TSrc, TDst>(TSrc source) => Task.Run<TDst>(() => (TDst)InternalMaker(source));
            protected Task InternalUpdaterAsync<TSrc, TDst>(TSrc source, TDst destination) => Task.Run(() => { InternalUpdater(source, destination); });

            protected IServiceProvider Provider { get; }
            protected IDataReader Reader => _reader ?? (_reader = Provider.GetService<IDataReader>());
            protected IEntorno Entorno => _entorno ?? (_entorno = Provider.GetService<IEntorno>());

            protected T Get<T>(Guid id) where T : Entity => Reader.Get<T>(id);

            protected Guid GetIdUsuario() => Entorno.IdUsuario;

            protected TDst GetObject<TSrc, TDst>(TSrc obj)
                where TSrc: class
                where TDst: class
            {
                if (obj == null)
                    return null;
                var mapperProp = _mapperFactory.GetMaker<TSrc, TDst>();
                return mapperProp.MakeAsync(obj).Result;
            }

            protected IEnumerable<T> GetEnumerable<T, TEntity>(ILista<TEntity> lista)
                where T: class
                where TEntity: Entity
            {
                var mapper = _mapperFactory.GetMaker<TEntity, T>();
                var enumerable = new List<T>();
                foreach (var item in lista)
                {
                    enumerable.Add(mapper.MakeAsync(item).Result);
                }
                return enumerable;
            }

            protected ILista<TEntity> GetLista<TEntity, T>(IEnumerable<T> enumerable)
                where T : class
                where TEntity : Entity
            {
                var maker = _mapperFactory.GetMaker<T, TEntity>();
                var lista = new Lista<TEntity>();
                foreach (var item in enumerable)
                {
                    lista.Add(maker.MakeAsync(item).Result);
                }
                return lista;
            }

            protected string GetEnumString<T>(T value)
                where T: Enum
            {
                return Enum.GetName(typeof(T), value);
            }

            protected T GetStringEnum<T>(string value)
                where T: Enum
            {
                if (string.IsNullOrEmpty(value)) {
                    return (T)Enum.GetValues(typeof(T)).GetValue(0);
                }
                return (T)Enum.Parse(typeof(T), value);
            }
        }

        public enum CopyAction
        {
            Create,
            Update
        }
    }
}
