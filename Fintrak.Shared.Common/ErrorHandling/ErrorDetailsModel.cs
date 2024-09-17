using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fintrak.Shared.Common.ErrorHandling
{
    public class ErrorDetailsModel
    {
        public int StatusCode { get; }
        public string Message { get; }
        public string? Details { get; }

        public ErrorDetailsModel(int statusCode, string message, string? details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

}
