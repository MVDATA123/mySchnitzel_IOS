using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCloud.Models.Domain
{
    public enum CouponType
    {
        [Display(Name = "% - Prozent")]
        Percent = 1,
        [Display(Name = "€ - Wert")]
        Value = 2
    }
}