using System;
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

        [DisplayName("Маршрут перевозки")]
        public string DestinationPoint { get; set; }

        [DisplayName("Количество пассажиров")]
        public Nullable<short> PassengerAmount { get; set; }

        [DisplayName("Количество детей")]
        public Nullable<short> ChildAmount { get; set; }

        [DisplayName("Цель поездки")]
        public string TripPurpose { get; set; }

        [DisplayName("Место и время посадки пассажиров")]
        public string SeatPlace { get; set; }

        [DisplayName("Срок фрахтования (дней)")]
        public Nullable<short> TripDuration { get; set; }

        [DisplayName("ФИО пассажиров, номер телефона")]
        [DataType(DataType.MultilineText)]
        public string SecondedPeople { get; set; }

        [DisplayName("Номер приказа о командировании")]
        public string OrderNumber { get; set; }

        [DisplayName("Дата приказа о командировании")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> OrderDate { get; set; }

        [DisplayName("Название приказа о командировании")]
        public string OrderName { get; set; }
        
        [DisplayName("Подтвердить принятие заявки на тел./факс")]
        public string ConfirmTelFax { get; set; }
        [DisplayName("Подтвердить принятие заявки по электронной почте")]
        public string ConfirmEmail { get; set; }
    }
}