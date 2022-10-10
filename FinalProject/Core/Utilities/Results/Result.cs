using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public Result(bool success, string message):this(success)
        {
            Message = message;
        }
        public Result(bool success)  //constructor overloading,, 
        {
            Success = success;
        }

        public bool Success { get; } //=> throw new NotImplementedException();

        public string Message { get; }
    }
}
