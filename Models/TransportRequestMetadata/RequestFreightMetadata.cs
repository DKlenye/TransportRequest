using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Intranet.Models
{
    [MetadataType(typeof(RequestFreightMetadata))]
    public partial class RequestFreight
    {
    }

    public class RequestFreightMetadata
    {
        [DisplayName("Номер договора, контракта, приказа")]
        public string OrderNumber { get; set; }

        [DisplayName("Способ загрузки")]
        public string LoadingType { get; set; }

        [DisplayName("Адрес загрузки")]
        public string LoadingAddress { get; set; }

        [DisplayName("Контактное лицо грузоотправителя, тел")]
        public string ContactName { get; set; }

        [DisplayName("Ответственный за заказ, цех, тел")]
        public string Responsible { get; set; }

        [DisplayName("Тип транспортного средства")]
        public string VehicleType { get; set; }

        [DisplayName("Пункт назначения")]
        public string DestinationPoint { get; set; }
    }
}