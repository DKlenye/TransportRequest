using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Intranet.Models
{
    [MetadataType(typeof(RequestInternationalMetadata))]
    public partial class RequestInternational
    {
    }

    public class RequestInternationalMetadata
    {
        [DisplayName("Направление (экспорт/импорт), маршрут перевозки")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Way { get; set; }
        
        [DisplayName("Базис поставки по контракту")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string DeliveryBasis { get; set; }
        
        [DisplayName("Количество транспортных средств")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public Nullable<int> VehicleCount { get; set; }
        
        [DisplayName("Адрес загрузки,(почтовый индекс) наименование грузоотправителя на языке страны отгрузки")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Consignor { get; set; }
       
        [DisplayName("Контактное лицо/телефон (на загрузке)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string LoadingContactName { get; set; }
       
        [DisplayName("Таможня отправления, адрес выдачи экспортной декларации (EX-1)")]
        public string DepartureCustoms { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)] 
        [DisplayName("Дата доставки (не ранее 2 рабочих дней с даты загрузки)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }

        [DisplayName("Адрес доставки, (почтовый индекс) наименование грузополучателя")]
        public string DeliveryAddress { get; set; }

        [DisplayName("Контактное лицо/телефон (на выгрузке)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string UnloadingContactName { get; set; }

        [DisplayName("Таможня назначения")]
        public string ReturnCustoms { get; set; }

        [DisplayName("Наименование груза")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string CargoName { get; set; }

        [DisplayName("Опасный груз (номер ООН, паспорт безопасности)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string DangerCargo { get; set; }

        [DisplayName("№ упаковочного листа")]
        public string PackageListNumber { get; set; }

        [DisplayName("Код ТНВЭД")]
        public string Code { get; set; }

        [DisplayName("Количество грузовых мест")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string CargoPlaces { get; set; }


        [DisplayName("Требуемый тип транспортного средства")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string VehicleType { get; set; }

         [DisplayName("Грузоподъёмность транспортного средства, кг")]
         [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string VehicleCapacityTonns { get; set; }

        [DisplayName("Габариты груза (м), упаковка")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string CargoDimensions { get; set; }

        [DisplayName("Вес груза брутто, кг")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Brutto { get; set; }

        [DisplayName("Вес груза нетто, кг")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Netto { get; set; }

        [DisplayName("Объём груза, м3")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string CargoVolume { get; set; }
        

        [DisplayName("Способ загрузки и разгрузки")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string LoadingType { get; set; }

        [DisplayName("Стоимость груза")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string CargoCost { get; set; }

        [DisplayName("Возможность перегрузки груза")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string CargoOverloading { get; set; }

        [DisplayName("Информация о штабелируемости груза")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string CargoStackability { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public Nullable<System.DateTime> DeliveryDateEnd { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public Nullable<System.DateTime> RequestDateEnd { get; set; }


    }
}