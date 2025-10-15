using EmployeeManager.API.Models;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using Npgsql;
using System.Net;

namespace EmployeeManager.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode status;
            string message;
            List<string> errors = new();

            switch (ex)
            {
                case ValidationException validationEx:
                    status = HttpStatusCode.BadRequest;
                    message = "Validation failed";
                    errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList();
                    break;

                case BusinessException businessEx:
                    status = HttpStatusCode.BadRequest;
                    message = businessEx.Message;
                    break;

                case NotFoundException notFoundEx:
                    status = HttpStatusCode.NotFound;
                    message = notFoundEx.Message;
                    break;

                case PostgresException pgEx:
                    status = HttpStatusCode.BadRequest;
                    message = MapPostgresException(pgEx);
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var response = ApiResponse<object>.ErrorResponse(message, errors);

            await context.Response.WriteAsJsonAsync(response);
        }       

        private static string MapPostgresException(PostgresException ex)
        {
            return ex.SqlState switch
            {
                PostgresErrorCodes.UniqueViolation => MapUniqueViolation(ex),
                PostgresErrorCodes.ForeignKeyViolation => "The referenced entity does not exist.",
                PostgresErrorCodes.NotNullViolation => "A required field was not provided.",
                _ => "A database error occurred."
            };
        }

        private static string MapUniqueViolation(PostgresException ex)
        {
            return ex.ConstraintName switch
            {
                "employees_phone_key" => "An employee with this phone number already exists.",
                "employees_passport_number_key" => "This passport number is already registered.",
                "departments_name_key" => "A department with this name already exists.",
                "departments_phone_key" => "A department with this phone number already exists.",
                "companies_name_key" => "A company with this name already exists.",
                "companies_address_key" => "A company with this address already exists.",
                _ => "Unique constraint violation."
            };
        }

    }
}
