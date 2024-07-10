using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public class NotFoundEntityException: BaseException
    {
        public NotFoundEntityException()
            : this("Entidad no encontrada")
        {
        }

        public NotFoundEntityException(string msg)
            : base(msg)
        {
            Codigo = 40401;
        }
    }
}
