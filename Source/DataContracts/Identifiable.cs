using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Scheduler.DataContracts
{
    [DataContract]
    public abstract class Identifiable
    {
        [DataMember]
        [Key]
        public int Id
        {
            get;
            set;
        }
    }
}