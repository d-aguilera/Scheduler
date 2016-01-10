using Scheduler.Common;
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

        public string Encrypt()
        {
            var json = Helpers.ToJson(this);
            return Helpers.ToBase64(json);
        }

        public static Toast Decrypt(string toastToken)
        {
            var json = Helpers.FromBase64(toastToken);
            return Helpers.FromJson<Toast>(json);
        }
    }
}