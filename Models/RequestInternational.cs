//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Intranet.Models
{
    public partial class RequestInternational
    {
        public int RequestId { get; set; }
        public string Way { get; set; }
        public string DeliveryBasis { get; set; }
        public Nullable<int> VehicleCount { get; set; }
        public string Consignor { get; set; }
        public string LoadingContactName { get; set; }
        public string DepartureCustoms { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string UnloadingContactName { get; set; }
        public string ReturnCustoms { get; set; }
        public string CargoName { get; set; }
        public string DangerCargo { get; set; }
        public string PackageListNumber { get; set; }
        public string Code { get; set; }
        public string CargoDimensions { get; set; }
        public string Brutto { get; set; }
        public string Netto { get; set; }
        public string LoadingType { get; set; }
        public string CargoCost { get; set; }
        public string CargoOverloading { get; set; }
        public string CargoStackability { get; set; }
        public string VehicleType { get; set; }
        public string CargoPlaces { get; set; }
        public string Currency { get; set; }
        public Nullable<System.DateTime> DeliveryDateEnd { get; set; }
        public Nullable<System.DateTime> RequestDateEnd { get; set; }
        public string VehicleCapacityTonns { get; set; }
        public string CargoVolume { get; set; }
    
        public virtual Request Request { get; set; }
    }
    
}
