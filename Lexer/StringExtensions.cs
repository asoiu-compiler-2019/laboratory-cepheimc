using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Lexer
{
    public static class StringExtensions
    {
        public static char CharAt(this string str, int index)
        {
            if (index > str.Length - 1 || index < 0)
            {
                return '\0';
            }

            return str[index];
        }
    }
}
