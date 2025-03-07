using System.Collections.Generic;

namespace KekaBot.kiki.Services.Models
{
    public class ResponseModel<T>
    {
        public bool Succeeded { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}