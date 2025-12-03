using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingReservations.Core.Shared
{
    public class ApiResponse<T>
    {
        private string v;

        public ApiResponse(string v)
        {
            this.v = v;
        }

        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public int StatusCode { get; set; }
    }
}
