﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.IService
{
    interface ISave
    {
        bool save<T>(T t);
    }
}
