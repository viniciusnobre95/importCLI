using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    public class BaseResponse
    {
        public string Codigo { get; set; }
        public bool Status { get; set; }
        public string Mensagem { get; set; }
        public string Resultado { get; set; }

    }
}
