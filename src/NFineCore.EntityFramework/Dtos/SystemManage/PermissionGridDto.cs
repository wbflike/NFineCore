using NFineCore.EntityFramework.Models.SystemManage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NFineCore.EntityFramework.Dtos.SystemManage
{

    public class PermissionGridDto
    {
        public string Id { get; set; }
        public string ResourceId { get; set; }
        public string ObjectId { get; set; }
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
