using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HS
{
    public class SwaggerJwtFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            if (context.MethodInfo.GetCustomAttribute(typeof(AuthorizeAttribute)) is AuthorizeAttribute)
            {
                AgregarJwt(operation);
            }
            else if (context.MethodInfo.DeclaringType.GetCustomAttribute<AuthorizeAttribute>() is AuthorizeAttribute)
            {
                if (context.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>() == null)
                {
                    AgregarJwt(operation);
                }
            }
            else if (context.MethodInfo.DeclaringType.IsGenericType && context.MethodInfo.DeclaringType.GetGenericTypeDefinition() == typeof(CrudController<>))
            {
                AgregarJwt(operation);
            }
        }

        private void AgregarJwt(OpenApiOperation operation)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "Token de autenticacion",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLz...")
                }
            });
        }
    }
}
