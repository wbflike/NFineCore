using System;
using System.Collections.Generic;
using System.Text;

namespace NFineCore.EntityFramework.Dtos.SystemManage
{
    public class UserInputDto
    {
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public byte? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string TelePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string WeChat { get; set; }
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
        public string RoleId { get; set; }
        public string DutyId { get; set; }
        public byte? Type { get; set; }
        public bool? EnabledMark { get; set; }
        public string Description { get; set; }
    }
}
