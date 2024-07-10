using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Logging
{
    public class Raiz : Entity
    {
        public Raiz()
        {
            Detalles = new Lista<Detalle>();
        }

        public virtual string IpOrigen { get; set; }
        public virtual string Browser { get; set; }
        public virtual string Request { get; set; }
        public virtual string Response { get; set; }
        public virtual DateTime FechaHora { get; set; }
        public virtual bool Error { get; set; }
        public virtual ILista<Detalle> Detalles { get; set; }
    }
}
