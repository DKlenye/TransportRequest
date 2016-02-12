using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Intranet.Validation
{
    public class NotZero : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (Int32)value != 0;
        }   
    }
}