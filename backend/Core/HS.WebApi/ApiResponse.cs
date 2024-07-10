using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
