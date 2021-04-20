using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace VacationRental.Api.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; internal set; }
        public string Message { get; internal set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
