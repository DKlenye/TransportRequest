using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Intranet.Validation;

namespace Intranet.Models
{
    [MetadataType(typeof(RequestMetadata))]
    public partial class Request
    {
        public string DetailsLink { get; set; }
        public string DetailsButton { get; set; }
        public string RequestTypeLabel { get; set; }
        public string Customer
        {
            get
            {
                if (this.v_RequestCustomer == null)
                    return String.Empty;
                else
                    return this.v_RequestCustomer.CustomerName;
            }
        }
    }

    public class RequestMetadata
    {
        [DisplayName("Выделить транспорт на дату")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Необходимо указать дату поездки")]
        //[FutureDate(ErrorMessage = "Поле не должно быть датой из прошлого.")]
        public Nullable<System.DateTime> RequestDate { get; set; }

        [DisplayName("Статус")]
        public Nullable<int> Status { get; set; }

        [DisplayName("Создана пользователем")]
        public string UserFio { get; set; }

        [DisplayName("Одобрено пользователем")]
        public string ApproverFio { get; set; }

        [DisplayName("Запись создана")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)] 
        public Nullable<System.DateTime> PublishDate { get; set; }

        //[NotZero]
        public Nullable<int> ApproverEmployeeId { get; set; }

        [DisplayName("Наименование заказчика")]
        public Nullable<int> CustomerId { get; set; }
    }
}