using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFineCore.EntityFramework.Dtos.SystemManage
{
    public class DutyInputDto
    {
        public string OrganizeId { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public bool? AllowEdit { get; set; }
        public bool? AllowDelete { get; set; }
        public int? SortCode { get; set; }
        public bool? EnabledMark { get; set; }
        public string Description { get; set; }
    }
}
