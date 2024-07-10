using HS.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HS
{
    public class LogService : ILogService
    {
        public const string LogServiceKey = "LOG_SERVICE_KEY";

        public static LogService GetService()
        {
            return CallContext<LogService>.GetData(LogServiceKey);
        }

        public Raiz Raiz { get; }

        public LogService(HttpRequestMessage request)
        {
            Raiz = new Raiz
            {
                FechaHora = DateTime.Now,
                Request = request.RequestUri.AbsoluteUri
            };

            CallContext<LogService>.SetData(LogServiceKey, this);
        }

        private void SetDetalle(Type clase, MethodInfo metodo, object[] parametros, string resultado)
        {
            var detalle = new Detalle
            {
                Clase = clase.Name,
                Metodo = metodo.Name,
                Parametros = JsonConvert.SerializeObject(parametros),
                Salida = resultado,
                Orden = Raiz.Detalles.Count
            };
            Raiz.Detalles.Add(detalle);
        }

        public void Detalle(Type clase, MethodInfo metodo, object[] parametros, object resultado)
        {
            SetDetalle(clase, metodo, parametros, JsonConvert.SerializeObject(resultado));
        }

        public void Error(Type clase, MethodInfo metodo, object[] parametros, Exception error)
        {
            StringBuilder err = new StringBuilder();
            err.AppendLine(error.Message);
            err.AppendLine();
            err.AppendLine(error.StackTrace);

            SetDetalle(clase, metodo, parametros, err.ToString());

            Raiz.Error = true;
        }

        public async Task SetResponse(Task<HttpResponseMessage> response)
        {
            Raiz.Response = await response.Result.Content.ReadAsStringAsync();
        }
    }
}
