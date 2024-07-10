using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace HS
{
    public class GlobalErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorMiddleware> _logger;

        public GlobalErrorMiddleware(RequestDelegate next, ILogger<GlobalErrorMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "ERROR DE SERVIDOR");
                await HandlerError(context, ex);
            }
        }

        private Task HandlerError(HttpContext context, Exception error)
        {
            context.Response.ContentType = "application/json";

            object data;
            if (error is BaseException ex)
            {
                context.Response.StatusCode = ex.Codigo / 100;
                context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ex.Message;
                data = ex.GetErrors();
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Error del Servidor";
                data = "Error del Servidor";
            }
            return context.Response.WriteAsync(JsonSerializer.Serialize(data));
        }
    }
}
