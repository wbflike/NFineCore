
using AutoMapper;
using NFineCore.Support;
using NFineCore.EntityFramework;
using NFineCore.EntityFramework.Dtos.SystemManage;
using NFineCore.EntityFramework.Models.SystemManage;
using NFineCore.Repository.SystemManage;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFineCore.Service.SystemManage
{
    public class OperateLogService
    {
        OperateLogRepository operateLogRepository = new OperateLogRepository(SharpRepoConfig.sharpRepoConfig,"efCore");

        public List<OperateLogGridDto> GetList(Pagination pagination, string keyword)
        {
            var specification = new Specification<OperateLog>(u => u.DeletedMark == false);
            var pagingOptions = new PagingOptions<OperateLog, DateTime?>(pagination.page, pagination.rows, x => x.OperateTime, isDescending: true);
            if (!string.IsNullOrEmpty(keyword))
            {
                specification = new Specification<OperateLog>(u => u.DeletedMark == false && (u.UserName.Contains(keyword)));
            }
            var list = operateLogRepository.FindAll(specification, pagingOptions).ToList();
            pagination.records = pagingOptions.TotalItems;
            return Mapper.Map<List<OperateLogGridDto>>(list);
        }

        public void SubmitForm(OperateLogInputDto operateLogInputDto, string keyValue)
        {
            OperateLog operateLog = new OperateLog();
            if (!string.IsNullOrEmpty(keyValue))
            {

            }
            else
            {
                AutoMapper.Mapper.Map<OperateLogInputDto, OperateLog>(operateLogInputDto, operateLog);
                operateLog.Id = IdWorkerHelper.GenId64();
                operateLog.DeletedMark = false;
                operateLog.CreationTime = DateTime.Now;
                operateLogRepository.Add(operateLog);
            }
        }

        public void TestAsync1()
        {
            OperateLog operateLog = new OperateLog();
            operateLog.Area = "hangfire";
            operateLog.Controller = "OperateLog";
            operateLog.Action = "TaskTest1";
            operateLog.Method = "Task";
            operateLog.UserId = 1035359633864265728;
            operateLog.UserName = "flyang";
            operateLog.Id = IdWorkerHelper.GenId64();
            operateLog.DeletedMark = false;
            operateLog.CreationTime = DateTime.Now;
            operateLogRepository.Add(operateLog);
        }
        public void TestAsync2()
        {
            OperateLog operateLog = new OperateLog();
            operateLog.Area = "hangfire";
            operateLog.Controller = "OperateLog";
            operateLog.Action = "TaskTest2";
            operateLog.Method = "Task";
            operateLog.UserId = 1035359633864265728;
            operateLog.UserName = "flyang";
            operateLog.Id = IdWorkerHelper.GenId64();
            operateLog.DeletedMark = false;
            operateLog.CreationTime = DateTime.Now;
            operateLogRepository.Add(operateLog);
        }
        public void TestAsync3()
        {
            OperateLog operateLog = new OperateLog();
            operateLog.Area = "hangfire";
            operateLog.Controller = "OperateLog";
            operateLog.Action = "TaskTest3";
            operateLog.Method = "Task";
            operateLog.UserId = 1035359633864265728;
            operateLog.UserName = "flyang";
            operateLog.Id = IdWorkerHelper.GenId64();
            operateLog.DeletedMark = false;
            operateLog.CreationTime = DateTime.Now;
            operateLogRepository.Add(operateLog);
        }
        public void TestAsync4()
        {
            OperateLog operateLog = new OperateLog();
            operateLog.Area = "hangfire";
            operateLog.Controller = "OperateLog";
            operateLog.Action = "TaskTest4";
            operateLog.Method = "Task";
            operateLog.UserId = 1035359633864265728;
            operateLog.UserName = "flyang";
            operateLog.Id = IdWorkerHelper.GenId64();
            operateLog.DeletedMark = false;
            operateLog.CreationTime = DateTime.Now;
            operateLogRepository.Add(operateLog);
        }
        public void TestAsync5()
        {
            OperateLog operateLog = new OperateLog();
            operateLog.Area = "hangfire";
            operateLog.Controller = "OperateLog";
            operateLog.Action = "TaskTest5";
            operateLog.Method = "Task";
            operateLog.UserId = 1035359633864265728;
            operateLog.UserName = "flyang";
            operateLog.Id = IdWorkerHelper.GenId64();
            operateLog.DeletedMark = false;
            operateLog.CreationTime = DateTime.Now;
            operateLogRepository.Add(operateLog);
        }
    }
}

