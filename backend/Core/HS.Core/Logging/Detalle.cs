using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Logging
{
    public class Detalle : Entity
    {
        public virtual int Orden { get; set; }
        public virtual string Clase { get; set; }
        public virtual string Metodo { get; set; }
        public virtual string Parametros { get; set; }
        public virtual string Salida { get; set; }
    }
}
