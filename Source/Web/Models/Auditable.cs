using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Scheduler.Web.Models
{
    public abstract class Auditable : Identifiable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "AuditableCreatedName", ShortName = "AuditableCreatedShortName", ResourceType = typeof(Resources))]
        public DateTime Created
        {
            get;
            set;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "AuditableCreatedByName", ShortName = "AuditableCreatedByShortName", ResourceType = typeof(Resources))]
        public string CreatedBy
        {
            get;
            set;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "AuditableLastUpdatedName", ShortName = "AuditableLastUpdatedShortName", ResourceType = typeof(Resources))]
        public DateTime? LastUpdated
        {
            get;
            set;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "AuditableLastUpdatedByName", ShortName = "AuditableLastUpdatedByShortName", ResourceType = typeof(Resources))]
        public string LastUpdatedBy
        {
            get;
            set;
        }
    }
}