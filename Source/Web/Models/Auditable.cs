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
        public DateTime Created
        {
            get;
            set;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CreatedBy
        {
            get;
            set;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime? LastUpdated
        {
            get;
            set;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string LastUpdatedBy
        {
            get;
            set;
        }
    }
}