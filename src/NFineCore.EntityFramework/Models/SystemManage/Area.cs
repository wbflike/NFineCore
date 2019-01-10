using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NFineCore.EntityFramework.Models.SystemManage
{
    [Table("Sys_Area")]
    public class Area
    {
        [Key]
        public long Id { get; set; }
        public long ParentId { get; set; }
        public int? Layers { get; set; }
        public string FullName { get; set; }
        public string EnCode { get; set; }
        public string SimpleSpelling { get; set; }
        public int? SortCode { get; set; }
        public string Description { get; set; }
        public bool? DeletedMark { get; set; }
        public bool? EnabledMark { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
    }
}
