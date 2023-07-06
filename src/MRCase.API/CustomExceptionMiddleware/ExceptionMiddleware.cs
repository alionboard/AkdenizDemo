using Microsoft.AspNetCore.Http;
using MRCase.Application.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MRCase.API.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            switch (error)
            {
                case ArgumentException e:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedAccessException e:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case FileNotFoundException e:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(new ResponseDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = error.Message ?? "Error",
            }.ToString());
            
        }
    }
}
