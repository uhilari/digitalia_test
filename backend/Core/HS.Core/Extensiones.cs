using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace HS
{
    public static class Extensiones
    {
        public static T NoEsNull<T>(this T entidad, string nombre)
          where T : class
        {
            if (entidad == null)
                throw new ArgumentNullException(nombre);
            return entidad;
        }

        public static string NoEsNull(this string cadena, string nombre)
        {
            if (string.IsNullOrEmpty(cadena))
                throw new RequeridoException(nombre);
            return cadena;
        }

        public static Error ValidarRequerido(this string cadena, string nombre)
        {
            try
            {
                cadena.NoEsNull(nombre);
                return null;
            }
            catch (RequeridoException ex)
            {
                return ex.GetError();
            }
        }

        public static Error ValidarRequerido<TEntity>(this TEntity entidad, string mensaje = null)
          where TEntity : Entity
        {
            var msg = mensaje ?? string.Format("No se encuentra '{0}'", typeof(TEntity).Name);
            if (entidad == null)
                return new Error(40002, msg);
            return null;
        }

        public static Error ObjetoRequerido(this object entidad, string nombre)
        {
            var msg = string.Format("No se encuentra '{0}'", nombre);
            if (entidad == null)
                return new Error(40002, msg);
            return null;
        }

        public static Error ValidarExpresionRegular(this string valor, string expReg, string campo)
        {
            var regex = new Regex(expReg);
            var msg = string.Format("'{0}' no cumple con el formato adecuado", campo);
            if (!regex.IsMatch(valor))
                return new Error(40003, msg);
            return null;
        }

        public static Error ValidarGuid(this string valor, string campo)
        {
            var msg = string.Format("'{0}' no tiene el formato de ID adecuado", campo);
            if (!valor.EsGuid())
                return new Error(40004, msg);
            return null;
        }

        public static Error ValidarPositivo(this decimal numero, string campo)
        {
            var msg = string.Format("'{0}' debe ser mayor a cero", campo);
            if (!numero.EsPositivo())
                return new Error(40007, msg);
            return null;
        }

        public static void AppendError(this IList<Error> errores, Error error)
        {
            if (error != null)
                errores.Add(error);
        }

        public static string EncodeBase64(this string str)
        {
            return str
                .Replace("/", "-")
                .Replace("+", "_")
                .Replace("=", "");
        }

        public static string DecodeBase64(this string str)
        {
            while ((str.Length % 4) != 0)
            {
                str += "=";
            }
            return str
                .Replace("-", "/")
                .Replace("_", "+");
        }

        public static string Cadena(this Guid id)
        {
            return Convert.ToBase64String(id.ToByteArray())
              .Replace("/", "-")
              .Replace("+", "_")
              .Replace("=", "");
        }

        public static string Cadena(this Guid? id)
        {
            if (id.HasValue)
                return Cadena(id.Value);
            return null;
        }

        public static bool EsGuid(this string cadena)
        {
            if (string.IsNullOrEmpty(cadena))
                return false;
            if (cadena.Length != 22)
                return false;
            return Regex.IsMatch(cadena, "[a-zA-Z0-9_-]{22}");
        }

        public static Guid Guid(this string cadena)
        {
            if (string.IsNullOrEmpty(cadena)) throw new ArgumentNullException(nameof(cadena));
            if (!EsGuid(cadena)) throw new GuidFormatException(cadena);

            var tmp = cadena.Replace("-", "/")
              .Replace("_", "+");
            return new Guid(Convert.FromBase64String(tmp + "=="));
        }

        public static Guid? GuidONull(this string cadena)
        {
            if (string.IsNullOrEmpty(cadena))
                return null;

            return Guid(cadena);
        }

        public static string Cadena(this short numero, int longitud)
        {
            return Cadena((int)numero, longitud);
        }

        public static string Cadena(this int numero, int longitud)
        {
            if (!EsPositivo(numero)) throw new InvalidOperationException("Numero no puede ser negativo");
            if (!EsPositivo(longitud)) throw new InvalidOperationException("Longitud no puede ser negativo");

            string cadena = numero.ToString();
            while (cadena.Length < longitud)
            {
                cadena = "0" + cadena;
            }
            return cadena;
        }

        public static bool EsPositivo(this int numero)
        {
            return (numero >= 0);
        }

        public static bool EsPositivo(this long numero)
        {
            return (numero >= 0L);
        }

        public static bool EsPositivo(this float numero)
        {
            return (numero >= 0f);
        }

        public static bool EsPositivo(this decimal numero)
        {
            return (numero >= 0m);
        }

        public static ILista<T> ToLista<T>(this IEnumerable<T> enumerable) where T : Entity
        {
            var lista = new Lista<T>();
            lista.AddRange(enumerable);
            return lista;
        }

        public static bool EsLista(this Type tipo)
        {
            if (!tipo.IsGenericType)
                return false;

            var typeDef = tipo.GetGenericTypeDefinition();
            return (typeDef != null) && (typeDef == typeof(ILista<>));
        }

        public static bool EsHijo(this Type tipo, Type tipoBase)
        {
            return tipoBase.IsAssignableFrom(tipo);
        }

        public static long ParteEntera(this decimal numero)
        {
            return Convert.ToInt64(Math.Truncate(numero));
        }

        public static object Invocar(this MethodInfo method, object obj, params object[] prms)
        {
            try
            {
                return method.Invoke(obj, prms);
            }
            catch(TargetInvocationException ex)
            {
                if (ex.InnerException is BaseException)
                    throw ex.InnerException;
                throw;
            }
        }

        public static void SetProvider(this RootEntity entity, IServiceProvider provider)
        {
            entity.Provider = provider;
        }

        public static DateTime FromJsDateTime(this long dateTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(dateTime)
                .ToLocalTime();
        }

        public static DateTime FromJsDate(this long dateTime)
        {
            return dateTime.FromJsDateTime().Date;
        }

        public static DateTime? FromJsDateTime(this long? dateTime)
        {
            if (!dateTime.HasValue)
                return null;
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(dateTime.Value)
                .ToLocalTime();
        }

        public static DateTime? FromJsDate(this long? dateTime)
        {
            return dateTime.FromJsDateTime()?.Date;
        }

        public static DateTime FromJsDateTimeUtc(this long dateTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(dateTime);
        }

        public static DateTime FromJsDateUtc(this long dateTime)
        {
            return dateTime.FromJsDateTimeUtc().Date;
        }

        public static DateTime? FromJsDateTimeUtc(this long? dateTime)
        {
            if (!dateTime.HasValue)
                return null;
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(dateTime.Value);
        }

        public static DateTime? FromJsDateUtc(this long? dateTime)
        {
            return dateTime.FromJsDateTimeUtc()?.Date;
        }

        public static DateTime AddTime(this DateTime fecha, string tiempo)
        {
            var intervalos = tiempo.Split(':');
            return fecha.Date
                .AddHours(int.Parse(intervalos[0]))
                .AddMinutes(int.Parse(intervalos[1]));
        }

        public static IEnumerable<TextoValor> ListValues(this Type tipo)
        {
            if (!tipo.IsEnum)
                throw new InvalidOperationException("Solo se puede aplicar a tipos Enum");
            var values = Enum.GetValues(tipo);
            var texts = Enum.GetNames(tipo);
            var result = new List<TextoValor>();
            for(int i=0; i<values.Length; i++)
            {
                result.Add(new TextoValor
                {
                    Valor = (int)values.GetValue(i),
                    Texto = texts[i]
                });
            }
            return result;
        }
    }
}
