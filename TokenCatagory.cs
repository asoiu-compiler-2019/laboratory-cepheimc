﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public enum TokenCatagory
    {
        Unknown,
        WhiteSpace,

        Constant,
        Identifier,
        Grouping,
        Punctuation,
        Operator,

        Invalid,
    }
}
