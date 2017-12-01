using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviseConverter.Utility
{
    public static class Utilities
    {
        public static T iif<T>(bool cond, T trueValue, T falseValue)
        {
            return (cond) ? trueValue : falseValue;
        }
    }
}