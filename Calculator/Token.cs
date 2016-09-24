﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class Token
    {
        private string value;
        private TokenType type;

        public string Value { get { return value; } }
        public TokenType Type { get { return type; } }

        public Token(string value, TokenType type)
        {
            this.value = value;
            this.type = type;
        }
    }

    public enum TokenType
    {
        Variable,
        Operation
    }
}