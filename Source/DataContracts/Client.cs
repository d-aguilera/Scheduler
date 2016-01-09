using Scheduler.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.DataContracts
{
    [DataContract]
    public class Client : Auditable
    {
        [DataMember]
        [Required]
        [Display(Name = "ClientNetworkNameName", ShortName = "ClientNetworkNameShortName", ResourceType = typeof(Resources))]
        public string NetworkName
        {
            get;
            set;
        }

        [DataMember]
        [Display(Name = "ClientDescriptionName", ShortName = "ClientDescriptionShortName", ResourceType = typeof(Resources))]
        public string Description
        {
            get;
            set;
        }

        [DataMember]
        [Display(Name = "ClientAgentPortName", ShortName = "ClientAgentPortShortName", ResourceType = typeof(Resources))]
        public int? AgentPort
        {
            get;
            set;
        }

        [DataMember]
        [Display(Name = "ClientAgentVirtualDirectoryName", ShortName = "ClientVirtualDirectoryShortName", ResourceType = typeof(Resources))]
        public string AgentVirtualDirectory
        {
            get;
            set;
        }
    }
}
