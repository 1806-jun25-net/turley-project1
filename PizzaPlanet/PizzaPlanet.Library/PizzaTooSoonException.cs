using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class PizzaTooSoonException : Exception
    {
        public PizzaTooSoonException()
        {
        }

        public PizzaTooSoonException(string message)
            : base(message)
        {
        }

        public PizzaTooSoonException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
