using Scheduler.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Scheduler.DataContracts
{
    [DataContract]
    public abstract class Auditable : Identifiable
    {
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "AuditableCreatedName", ShortName = "AuditableCreatedShortName", ResourceType = typeof(Resources))]
        public DateTime Created
        {
            get;
            set;
        }

        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "AuditableCreatedByName", ShortName = "AuditableCreatedByShortName", ResourceType = typeof(Resources))]
        public string CreatedBy
        {
            get;
            set;
        }

        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "AuditableLastUpdatedName", ShortName = "AuditableLastUpdatedShortName", ResourceType = typeof(Resources))]
        public DateTime? LastUpdated
        {
            get;
            set;
        }

        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "AuditableLastUpdatedByName", ShortName = "AuditableLastUpdatedByShortName", ResourceType = typeof(Resources))]
        public string LastUpdatedBy
        {
            get;
            set;
        }
    }
}