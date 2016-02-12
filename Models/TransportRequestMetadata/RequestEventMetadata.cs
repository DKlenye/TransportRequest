using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intranet.Models
{
    [MetadataType(typeof(RequestEventMetadata))]
    public partial class RequestEvent
    {
    }

    public class RequestEventMetadata
    {   
        [DisplayName("Сообщение диспетчера")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}