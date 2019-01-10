using System;
using System.Collections.Generic;
using System.Text;

namespace NFineCore.EntityFramework.Dtos.SystemManage
{
    public class LoginLogInputDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? OperateTime { get; set; }
        public string OperateType { get; set; }
        public bool? OperateResult { get; set; }
        public string IpAddress { get; set; }
        public string IpAddressLocation { get; set; }
        public bool? DeletedMark { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Description { get; set; }
    }
}
