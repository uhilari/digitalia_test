using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class Error
    {
        public Error(int codigo, string mensaje)
        {
            Codigo = codigo;
            Mensaje = mensaje;
        }

        public int Codigo { get; }
        public string Mensaje { get; }
    }
}
