using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFineCore.EntityFramework.Models.SystemManage
{
    [Table("Sys_OperateLog")]
    public class OperateLog
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Method { get; set; }
        public DateTime? OperateTime { get; set; }
        public string Parameter { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }        
        public bool? DeletedMark { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
        public string Description { get; set; }
    }
}
