using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Crud.Commands
{
    public class CreateCommand<Dto> : Command
      where Dto : class
    {
        public Dto Data { get; set; }
    }
}
