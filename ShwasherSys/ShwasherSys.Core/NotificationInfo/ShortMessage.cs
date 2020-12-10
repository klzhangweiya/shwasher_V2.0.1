using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace ShwasherSys.NotificationInfo
{
    

    [Table("ShortMessage")]
    public  class ShortMessage:Entity<int>
    {
        public const int SendUserIDMaxLength = 20;
        public const int TitleMaxLength = 100;
        public const int ContentMaxLength = 3000;
        public const int IsDeleteMaxLength = 1;
        public const int RecieveUserIdsMaxLength = 400;
        /*[Key]
        public int MsgID { get; set; }*/

        [Required]
        [StringLength(SendUserIDMaxLength)]
        public string SendUserID { get; set; }

        [StringLength(TitleMaxLength)]
        public string Title { get; set; }

        [StringLength(ContentMaxLength)]
        public string Content { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SendTime { get; set; }

        [Required]
        [StringLength(IsDeleteMaxLength)]
        public string IsDelete { get; set; }

        [StringLength(RecieveUserIdsMaxLength)]
        public string RecieveUserIds { get; set; }

    }
}
