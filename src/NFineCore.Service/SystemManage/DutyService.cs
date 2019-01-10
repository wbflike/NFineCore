using AutoMapper;
using NFineCore.EntityFramework.Dtos.SystemManage;
using NFineCore.EntityFramework.Models.SystemManage;
using NFineCore.Repository.SystemManage;
using NFineCore.Support;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFineCore.Service.SystemManage
{
    public class DutyService
    {
        DutyRepository dutyRepository = new DutyRepository(SharpRepoConfig.sharpRepoConfig, "efCore");

        public List<DutyGridDto> GetList()
        {
            var spec = new Specification<Duty>();
            spec = new Specification<Duty>(p => p.EnabledMark.Equals(true));
            var list = dutyRepository.FindAll(spec).ToList();
            return Mapper.Map<List<DutyGridDto>>(list);
        }

        public List<DutyGridDto> GetList(string keyword)
        {
            var spec = new Specification<Duty>(p => p.DeletedMark.Equals(false));
            if (!string.IsNullOrEmpty(keyword))
            {
                spec = new Specification<Duty>(p => p.DeletedMark.Equals(false) && (p.FullName.Contains(keyword) || p.EnCode.Contains(keyword)));
            }
            var sortingOtopns = new SortingOptions<Duty, int?>(x => x.SortCode, isDescending: false);
            var list = dutyRepository.FindAll(spec, sortingOtopns).ToList();
            return Mapper.Map<List<DutyGridDto>>(list);
        }

        public DutyOutputDto GetForm(string keyValue)
        {
            long id = Convert.ToInt64(keyValue);
            DutyOutputDto dutyOutputDto = new DutyOutputDto();
            Duty duty = dutyRepository.Get(id);
            AutoMapper.Mapper.Map<Duty, DutyOutputDto>(duty, dutyOutputDto);
            return dutyOutputDto;
        }

        public void SubmitForm(DutyInputDto dutyInputDto, string keyValue)
        {
            Duty duty = new Duty();
            if (!string.IsNullOrEmpty(keyValue))
            {
                var id = Convert.ToInt64(keyValue);
                duty = dutyRepository.Get(id);
                AutoMapper.Mapper.Map<DutyInputDto, Duty>(dutyInputDto, duty);
                duty.LastModificationTime = DateTime.Now;
                dutyRepository.Update(duty);
            }
            else
            {
                AutoMapper.Mapper.Map<DutyInputDto, Duty>(dutyInputDto, duty);
                duty.Id = IdWorkerHelper.GenId64();
                duty.CreationTime = DateTime.Now;
                duty.CreatorUserId = 1;
                dutyRepository.Add(duty);
            }
        }
        public void DeleteForm(string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            Duty duty = dutyRepository.Get(id);
            duty.DeletedMark = true;
            duty.DeletionTime = DateTime.Now;
            dutyRepository.Update(duty);
        }
    }
}
