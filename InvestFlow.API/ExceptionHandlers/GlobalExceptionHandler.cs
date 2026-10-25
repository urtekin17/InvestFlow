using InvestFlow.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestFlow.API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                CustomerAlreadyExistsException => StatusCodes.Status409Conflict,
                CustomerNotFoundException => StatusCodes.Status404NotFound,
                DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
                IdempotencyKeyConflictException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            httpContext.Response.StatusCode = statusCode;
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = statusCode switch
                {
                    409 => "Conflict",
                    404 => "Not Found",
                    _ => "Internal Server Error"
                },
                Detail = exception switch
                {
                    DbUpdateConcurrencyException =>
                        "The record was modified by another transaction. Please retry the operation.",

                    CustomerAlreadyExistsException =>
                        "A customer with this email already exists.",

                    CustomerNotFoundException =>
                        "Customer not found.",

                    IdempotencyKeyConflictException =>
                        "This idempotency key has already been used for a different order.",

                    _ => "An unexpected error occurred."
                }
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails,cancellationToken);

            return true;
        }
    }
}
