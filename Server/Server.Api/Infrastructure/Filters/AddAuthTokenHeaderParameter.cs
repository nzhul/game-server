using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Server.Api.Infrastructure.Filters
{
    public class AddAuthTokenHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            operation.Parameters.Add(new HeaderParameter()
            {
                Name = "Authorization",
                In = "header",
                Type = "string",
                Default = "Bearer ",
                Required = false
            });

        }
    }

    class HeaderParameter : NonBodyParameter
    {
    }
}
