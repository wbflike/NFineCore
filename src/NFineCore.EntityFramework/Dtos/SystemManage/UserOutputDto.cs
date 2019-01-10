using Newtonsoft.Json;
using NFineCore.EntityFramework.Models.SystemManage;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NFineCore.EntityFramework.Dtos.SystemManage
{
    public class UserOutputDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public byte? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string TelePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string WeChat { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string RoleIds { get; set; }
        public string RoleNames { get; set; }
        public string DutyId { get; set; }
        public string DutyName { get; set; }
        public byte? Type { get; set; }
        public bool? EnabledMark { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public List<UserRole> UserRoles { get; set; }
    }
}
