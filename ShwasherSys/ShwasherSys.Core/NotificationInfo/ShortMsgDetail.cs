using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ShwasherSys.NotificationInfo
{
    
    

    [Table("ShortMsgDetail")]
    public class ShortMsgDetail:Entity<int>
    {
        public const int RecvUserIDMaxLength = 20;
        public const int IsReadMaxLength = 1;
        /*[Key]
        public int DetailID { get; set; }*/

        public int MsgID { get; set; }

        [Required]
        [StringLength(RecvUserIDMaxLength)]
        public string RecvUserID { get; set; }

        [Required]
        [StringLength(IsReadMaxLength)]
        public string IsRead { get; set; }


     
    }
}
