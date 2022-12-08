﻿using OnlineStore.BLL.AccountService.Exceptions;
using System.Net.Mime;

namespace OnlineStore.middleware
{
    public class ErrorHandlerMiddlaware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddlaware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                ILogger logger = context.RequestServices.GetService<ILogger<ErrorHandlerMiddlaware>>();

                response.ContentType = MediaTypeNames.Text.Plain;

                switch (error)
                {
                    case UserNotFoundException:
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        break;
                    case UserFoundException:
                        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                        break;
                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;

                }

                logger.LogError(error.Message);
                await response.WriteAsync(error.Message);
            }
        }
    }
}