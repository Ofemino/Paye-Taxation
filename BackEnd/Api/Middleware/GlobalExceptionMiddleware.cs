using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using System.Collections.Generic;
using Paye.Shared;

namespace Paye.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = ApiResponse.Fail(exception.Message);

            switch (exception)
            {
                case ValidationException validationEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Validation Failed";
                    response.Errors = new Dictionary<string, List<string>>();
                    foreach (var error in validationEx.Errors)
                    {
                        if (!response.Errors.ContainsKey(error.PropertyName))
                        {
                            response.Errors[error.PropertyName] = new List<string>();
                        }
                        response.Errors[error.PropertyName].Add(error.ErrorMessage);
                    }
                    break;

                case KeyNotFoundException:
                case NullReferenceException when exception.Message.Contains("not found"):
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                    
                case InvalidOperationException:
                     context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                     break;

                default:
                    Log.Error(exception, "Unhandled exception occurred");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "An unexpected error occurred.";
                    break;
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
