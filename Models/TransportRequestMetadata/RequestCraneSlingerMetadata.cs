using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Intranet.Models
{
    [MetadataType(typeof(RequestCraneSlingerMetadata))]
    public partial class RequestCraneSlinger
    {
    }

    public class RequestCraneSlingerMetadata
    {
        [DisplayName("Должность")]   
        public string Office { get; set; }

        [DisplayName("ФИО")]
        public string FIO { get; set; }

        [DisplayName("Номер удостоверения")]
        public string CertificateNumber { get; set; }

        [DisplayName("Дата последней проверки знаний")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateKnowledge { get; set; }
    }
}