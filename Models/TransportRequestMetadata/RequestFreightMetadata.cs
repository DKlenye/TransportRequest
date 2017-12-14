
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

        [DisplayName("Способ погрузки и разгрузки (боковое, заднее, верхнее, полное расчехление и др.)")]
        public string LoadingType { get; set; }

        [DisplayName("Адрес загрузки")]
        public string LoadingAddress { get; set; }

        [DisplayName("Контактное лицо на месте загрузки (Ф.И.О)/ номер телефона")]
        public string ContactName { get; set; }

        [DisplayName("Ответственный за заказ, цех, тел")]
        public string Responsible { get; set; }

        [DisplayName("Тип транспортного средства")]
        public string VehicleType { get; set; }

        [DisplayName("Адрес пункта разгрузки")]
        public string DestinationPoint { get; set; }

        [DisplayName("Время подачи")]
        public string LoadingTime { get; set; }
        [DisplayName("Грузоотправитель")]
        public string Shipper { get; set; }
        [DisplayName("Грузополучатель")]
        public string Consignee { get; set; }
        [DisplayName("Контактное лицо на месте разгрузки (Ф.И.О)/ номер телефона")]
        public string ConsigneeContactName { get; set; }
        [DisplayName("Срок доставки груза")]
        public string DeliveryTime { get; set; }
        [DisplayName("Количество транспортных средств")]
        public int VehicleCount { get; set; }
        [DisplayName("Грузоподъёмность транспортных средств, тонн")]
        public int VehicleCapacityTonns { get; set; }
        [DisplayName("Наличие постоянной  мобильной связи обязательно")]
        public bool MobileConnection { get; set; }
        [DisplayName("Сообщать о прибытии на загрузку/разгрузку; ежедневное местонахождение грузового транспортного средства")]
        public bool LocationInfo { get; set; }
        [DisplayName("Незамедлительно информировать заказчика о всех изменениях в процессе выполнения перевозки")]
        public bool ChangesInfo { get; set; }

    }
}