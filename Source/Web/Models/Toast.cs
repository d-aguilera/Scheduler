using Newtonsoft.Json;
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
            var json = JsonConvert.SerializeObject(this);
            return Helpers.ToBase64(json);
        }

        public static Toast Decrypt(string toastToken)
        {
            var json = Helpers.FromBase64(toastToken);
            return JsonConvert.DeserializeObject<Toast>(json);
        }
    }
}