using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public interface IUniqueQuery<TEntity, TUniqueKey>: IQuery<bool>
        where TEntity: Entity
    {
        TUniqueKey Key { get; set; }
    }
}
