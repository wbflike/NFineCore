using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NFineCore.EntityFramework.Models.SystemManage
{
    [Table("Sys_Permission")]
    public class Permission
    {
        [Key]
        public long Id { get; set; }
        public long ResourceId {get;set;}
        [ForeignKey("ResourceId")]
        public Resource Resource { get; set; }
        public long ObjectId { get; set; }
        public string ObjectType { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
        public int? SortCode { get; set; }
        public bool? DeletedMark { get; set; }
    }
}
