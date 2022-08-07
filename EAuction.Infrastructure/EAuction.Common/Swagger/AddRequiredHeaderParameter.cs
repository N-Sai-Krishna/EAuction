using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAuction.Common.Swagger
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(
                new OpenApiParameter {
                Name = "authoize-key",
                In = ParameterLocation.Header,
                Description = "Authorization Header",
                Required = true
                });

        }
    }
}
