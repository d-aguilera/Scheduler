using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler.Web.Models
{
    public class Toast
    {
        public string Title
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public ToastTypes Type
        {
            get;
            set;
        }
    }
}