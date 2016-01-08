using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scheduler.Web.Models
{
    public class Client : Auditable
    {
        [Required]
        public string NetworkName
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}