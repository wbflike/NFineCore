using System;
using System.Collections.Generic;
using System.Text;

namespace NFineCore.EntityFramework.Dtos.SystemManage
{
    public class RoleGridDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string EnCode { get; set; }
        public int? SortCode { get; set; }
        public bool? AllowEdit { get; set; }
        public bool? AllowDelete { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool? EnabledMark { get; set; }
        public DateTime? CreationTime { get; set; }
        public string OrganizeId { get; set; }
    }
}
