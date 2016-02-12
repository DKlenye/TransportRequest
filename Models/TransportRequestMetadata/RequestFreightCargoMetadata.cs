using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        [DisplayName("Вес, т")]
        [DisplayFormat(DataFormatString = "{0:f2}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> Weight { get; set; }

        [DisplayName("Длина, м")]
        public Nullable<decimal> Length { get; set; }

        [DisplayName("Ширина, м")]
        public Nullable<decimal> Width { get; set; }

        [DisplayName("Высота, м")]
        public Nullable<decimal> Height { get; set; }

        [DisplayName("Объем, м3")]
        public Nullable<decimal> Volume { get; set; }
    }
}