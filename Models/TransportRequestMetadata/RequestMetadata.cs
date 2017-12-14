using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Intranet.Models
{
    [MetadataType(typeof(RequestMetadata))]
    public partial class Request
    {
        public string DetailsLink { get; set; }
        public string DetailsButton { get; set; }
        public string CopyButton { get; set; }
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
        public DateTime? RequestDate { get; set; }

        [DisplayName("Статус")]
        public int? Status { get; set; }

        [DisplayName("Создана пользователем")]
        public string UserFio { get; set; }

        [DisplayName("Одобрено пользователем")]
        public string ApproverFio { get; set; }

        [DisplayName("Запись создана")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)] 
        public DateTime? PublishDate { get; set; }

        //[NotZero]
        public int? ApproverEmployeeId { get; set; }

        [DisplayName("Наименование заказчика (цель)")]
        public int? CustomerId { get; set; }

        [DisplayName("Наименование структурного подразделения")] 
        public int? DirectionId { get; set; }
        [DisplayName("Цель")]
        public int? AgreementPurposeId { get; set; }
        [DisplayName("Подразделение")]
        public int? DepartmentGroupId { get; set; }

        [DisplayName("Другие дополнительные сведения")]
        [DataType(DataType.MultilineText)]
        public string OtherInformation { get; set; }

       [DisplayName("Ответственный за использование транспорта")]
        public string Responsible { get; set; }
    }
}