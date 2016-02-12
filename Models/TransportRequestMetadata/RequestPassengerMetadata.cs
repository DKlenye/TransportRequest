using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Intranet.Models
{
    [MetadataType(typeof(RequestPassengerMetadata))]
    public partial class RequestPassenger
    {
    }

    public class RequestPassengerMetadata
    {

        [DisplayName("Пункт назначения")]
        public string DestinationPoint { get; set; }

        [DisplayName("Количество пассажиров")]
        public Nullable<short> PassengerAmount { get; set; }

        [DisplayName("Количество детей")]
        public Nullable<short> ChildAmount { get; set; }

        [DisplayName("Цель поездки")]
        public string TripPurpose { get; set; }

        [DisplayName("Место и время посадки")]
        public string SeatPlace { get; set; }

        [DisplayName("Срок командировки (дней)")]
        public Nullable<short> TripDuration { get; set; }

        [DisplayName("ФИО командируемых, номер телефона")]
        [DataType(DataType.MultilineText)]
        public string SecondedPeople { get; set; }

        [DisplayName("Номер приказа о командировании")]
        public string OrderNumber { get; set; }

        [DisplayName("Дата приказа о командировании")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> OrderDate { get; set; }

        [DisplayName("Название приказа о командировании")]
        public string OrderName { get; set; }
    }
}