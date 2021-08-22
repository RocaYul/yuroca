using System;
using System.Collections.Generic;
using System.Text;

namespace yuroca.common.Responses
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        //Si llamado al api fue correcto
        public string Message { get; set; }
        //Si pudimos o no llamado al api
        public object Result { get; set; }
        //Dependiendo el resultado devulve un valor
    }
}
