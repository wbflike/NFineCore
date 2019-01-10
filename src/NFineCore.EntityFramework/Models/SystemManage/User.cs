using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFineCore.EntityFramework.Models.SystemManage
{
    [Table("Sys_User")]
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SecretKey { get; set; }
        public string NickName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string TelePhone { get; set; }
        public string MobilePhone { get; set; }
        public string WeChat { get; set; }
        public DateTime? Birthday { get; set; }
        public byte? Type { get; set; }
        public byte? Gender { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool? EnabledMark { get; set; }
        public bool? DeletedMark { get; set; }
        public bool? IsAdministrator { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
        public long CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Organize Company { get; set; }
        public long DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Organize Department { get; set; }
        public long DutyId { get; set; }
        [ForeignKey("DutyId")]
        public Duty Duty { get; set; }
        public string Description { get; set; }
    }
}
