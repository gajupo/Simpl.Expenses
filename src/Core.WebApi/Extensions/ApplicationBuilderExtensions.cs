using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.WebApi.Middlewares;

namespace Core.WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}