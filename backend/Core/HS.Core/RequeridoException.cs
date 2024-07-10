using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class RequeridoException : BaseException
    {
        public RequeridoException(string nombre)
          : base(string.Format("Se debe proporcionar un valor para '{0}'", nombre))
        {
            Codigo = 40001;
        }
    }
}
