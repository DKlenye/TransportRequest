using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intranet.Models
{
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
        /// <summary>
        /// ФИО в формате Иванов И.И.
        /// </summary>
        public string Fio
        {
            get
            {
                string ln = this.LastName;
                ln = ln.Substring(0, 1) + ln.ToLower().Substring(1, ln.Length - 1);
                string approverFio = ln + " " + this.FirstName.Substring(0, 1) + "." + this.MiddleName.Substring(0, 1) + ".";
                return approverFio;
            }
        }
        
    }

    public class EmployeeMetadata
    {
    }
}