using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Intranet.Models
{
    [MetadataType(typeof(RequestCraneMetadata))]
    public partial class RequestCrane
    {
    }

    public class RequestCraneMetadata
    {
        [DisplayName("Номер лицензии, разрешение на право эксплуатации грузоподъемного крана")]
        public string LicenceNumber { get; set; }

        [DisplayName("Место проведения работ")]
        public string WorkPlace { get; set; }

        [DisplayName("Объект")]
        public string WorkObject { get; set; }

        [DisplayName("Характер (вид) работы")]
        public string WorkType { get; set; }

        [DisplayName("Тип подъемника")]
        public string CraneType { get; set; }

        [DisplayName("Наличие ЛЭП на месте работы")]
        public Nullable<bool> PowerLineExists { get; set; }

        [DisplayName("Разрешение организации, эксплуатирующей ЛЭП при работе в охранной зоне (кем выдано, номер, дата)")]
        public string PowerPermission { get; set; }

        [DisplayName("Лицо, ответственное за безопастное производство работ кранами (подъемниками). Должность, ФИО, номер удостоверения, дата последней проверки знаний")]
        public string Responsible { get; set; }

        [DisplayName("Наличие проекта или технологии производства работ")]
        public Nullable<bool> ProjectExists { get; set; }

        [DisplayName("Номер приказа о назначении ответственных лиц и стропальщиков")]
        public string ResponsibleOrder { get; set; }

        [DisplayName("Наименование организации, заявляющей кран")]
        public string CustomerName { get; set; }
    }
}