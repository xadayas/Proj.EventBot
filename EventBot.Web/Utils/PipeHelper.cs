using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBot.Web.Utils
{
    public static class PipeHelper
    {
        public static TOut Pipe<TIn, TOut>(this TIn _this, Func<TIn, TOut> func)
        {
            return func(_this);
        }
    }
}
