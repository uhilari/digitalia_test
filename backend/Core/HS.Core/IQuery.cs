﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface IQuery<TResultado>
    {
        Task<TResultado> ExecuteAsync();
    }
}
