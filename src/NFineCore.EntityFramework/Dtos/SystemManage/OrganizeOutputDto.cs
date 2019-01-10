using System;
using System.Collections.Generic;
using System.Text;

namespace NFineCore.EntityFramework.Dtos.SystemManage
{
    public class OrganizeOutputDto
    {
        public string ParentId { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public string TelePhone { get; set; }
        public string MobilePhone { get; set; }
        public string WeChat { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool? EnabledMark { get; set; }
        public string Description { get; set; }
        public int? SortCode { get; set; }
    }
}
