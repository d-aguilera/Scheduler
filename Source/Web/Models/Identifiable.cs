using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scheduler.Web.Models
{
    public abstract class Identifiable
    {
        [Key]
        public int Id
        {
            get;
            set;
        }
    }
}