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
    public partial class v_RequestType
    {
        public v_RequestType()
        {
            this.Request = new HashSet<Request>();
        }
    
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
    
        public virtual ICollection<Request> Request { get; set; }
    }
    
}
