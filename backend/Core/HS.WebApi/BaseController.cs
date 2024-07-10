using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public class BaseController : ControllerBase
    {
        public BaseController(ICommandExecutor executor)
        {
            Executor = executor;
        }

        protected ICommandExecutor Executor { get; }

        protected IActionResult Success<T>(T data)
        {
            return Ok(new ApiResponse<T>
            {
                Status = 200,
                Success = true,
                Data = data
            });
        }
    }
}
