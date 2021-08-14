using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace drugStore7.Web.Providers
{

    public struct CartResponse
    {
        public string Message { get; set; }
        public int CartItemCount { get; set; }
    }

}