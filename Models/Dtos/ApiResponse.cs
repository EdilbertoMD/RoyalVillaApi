using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RoyalVillaApi.Models.Dtos
{
    public class ApiResponse<TData>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public TData? Data { get; set; } 
        public object? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<TData> Create(bool success, int statusCode, string message, TData? data = default, object? errors = null)
        {
            return new ApiResponse<TData>
            {
                Success = success,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = errors,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ApiResponse<TData> NotFound(string message = "Recurso no encontrado")
        {
            return Create(false, (int)HttpStatusCode.NotFound, message);
        }

        public static ApiResponse<TData> BadRequest(string message, object? errors = null)
        {
            return Create(false, (int)HttpStatusCode.BadRequest, message, errors: errors);
        }

        public static ApiResponse<TData> Conflict(string message)
        {
            return Create(false, (int)HttpStatusCode.Conflict, message);
        }

        public static ApiResponse<TData> CreatedAt(string message, TData data)
        {
            return Create(true, (int)HttpStatusCode.Created, message, data);
        }

        public static ApiResponse<TData> Ok(string message, TData data)
        {
            return Create(true, (int)HttpStatusCode.OK, message, data);
        }

        public static ApiResponse<TData> NoContent(string message = "Operacion exitosa")
        {
            return Create(true, (int)HttpStatusCode.NoContent, message);
        }
        
        /*public static ApiResponse<TData> InternalServerError(string message, object? errors = null)
        {
            return Create(false, (int)HttpStatusCode.InternalServerError, message, default, errors);
        }*/

        public static ApiResponse<TData> Error(Exception ex, string message)
        {
            return Create(false, (int)HttpStatusCode.InternalServerError, message, default, ex.Message);
        }
    }
}