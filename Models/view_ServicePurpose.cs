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
    public partial class view_ServicePurpose
    {
        public view_ServicePurpose()
        {
            this.Requests = new HashSet<Request>();
        }
    
        public int AgreementPurposeId { get; set; }
        public int AgreementId { get; set; }
        public Nullable<int> ServiceDepartmentGroupId { get; set; }
        public string PurposeName { get; set; }
    
        public virtual ICollection<Request> Requests { get; set; }
    }
    
}