using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Intranet.Models
{
    [MetadataType(typeof(RequestFreightCargoMetadata))]
    public partial class RequestFreightCargo
    {
    }

    public class RequestFreightCargoMetadata
    {
        [DisplayName("Наименование")]
        public string CargoName { get; set; }

        [DisplayName("Вес брутто, тонн ")]
        [DisplayFormat(DataFormatString = "{0:f2}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> Weight { get; set; }

        [DisplayName("Длина, м")]
        public decimal Length { get; set; }

        [DisplayName("Ширина, м")]
        public Nullable<decimal> Width { get; set; }

        [DisplayName("Высота, м")]
        public Nullable<decimal> Height { get; set; }

        [DisplayName("Объем, м3")]
        public Nullable<decimal> Volume { get; set; }
        
        [DisplayName("Груз. мест")]
        public Nullable<int> NumberOfPackages { get; set; }
        [DisplayName("Вид упаковки")]
        public string KindOfPacking { get; set; }
         [DisplayName("Стоимость, руб")]
        public Nullable<decimal> Cost { get; set; }
         [DisplayName("Особые свойства груза")]
        public string SpecialProperties { get; set; }

    }
}