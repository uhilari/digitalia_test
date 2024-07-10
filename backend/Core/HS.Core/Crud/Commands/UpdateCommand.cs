using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Crud.Commands
{
    public class UpdateCommand<Dto> : Command
      where Dto : class
    {
        public string Id { get; set; }
        public Dto Data { get; set; }
    }
}
