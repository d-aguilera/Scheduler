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
        [Display(Name = "ClientNetworkNameName", ShortName = "ClientNetworkNameShortName", ResourceType = typeof(Resources))]
        public string NetworkName
        {
            get;
            set;
        }

        [Display(Name = "ClientDescriptionName", ShortName = "ClientDescriptionShortName", ResourceType = typeof(Resources))]
        public string Description
        {
            get;
            set;
        }
    }
}