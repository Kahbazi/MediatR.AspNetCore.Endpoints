using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InputValidation.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException validationException)
            {
                var errors = validationException.Errors
                    .Select(e => new Error { Property = e.PropertyName, ErrorMessage = e.ErrorMessage })
                    .ToArray();

                context.Response.StatusCode = 400;

                context.Response.Headers.Add("content-type", "application/json");

                await JsonSerializer.SerializeAsync(context.Response.Body, errors, errors.GetType(), null, context.RequestAborted);
                await context.Response.Body.FlushAsync(context.RequestAborted);
            }
        }


        private class Error
        {
            public string Property { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
