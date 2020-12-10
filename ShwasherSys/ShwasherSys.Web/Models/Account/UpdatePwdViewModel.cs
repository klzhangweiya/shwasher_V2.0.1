using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShwasherSys.Models.Account
{
    public class UpdatePwdViewModel
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string LoginPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}