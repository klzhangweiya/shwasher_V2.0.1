using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Validation;

namespace ShwasherSys.Dto
{
    public class CommonDto<T>: ICustomValidate
    {
        public T Key { get; set; }
      

        public void AddValidationErrors(CustomValidationContext context)
        {
            
        }
    }
}
