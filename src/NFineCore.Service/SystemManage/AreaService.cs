
using AutoMapper;
using NFineCore.Support;
using NFineCore.EntityFramework.Dtos.SystemManage;
using NFineCore.EntityFramework.Models.SystemManage;
using NFineCore.Repository.SystemManage;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFineCore.Service.SystemManage
{
    public class AreaService
    {
        AreaRepository areaRepository = new AreaRepository(SharpRepoConfig.sharpRepoConfig, "efCore");

        public List<AreaGridDto> GetList()
        {
            var specification = new Specification<Area>(u => u.DeletedMark == false);
            var sortingOtopns = new SortingOptions<Area, int?>(x => x.SortCode, isDescending: false);
            var list = areaRepository.FindAll(specification, sortingOtopns).ToList();
            return Mapper.Map<List<AreaGridDto>>(list);
        }

        public AreaOutputDto GetForm(string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            AreaOutputDto areaOutputDto = new AreaOutputDto();
            Area area = areaRepository.Get(id);
            AutoMapper.Mapper.Map<Area, AreaOutputDto>(area, areaOutputDto);
            return areaOutputDto;
        }

        public void SubmitForm(AreaInputDto areaInputDto, string keyValue)
        {
            Area area = new Area();
            if (!string.IsNullOrEmpty(keyValue))
            {
                var id = Convert.ToInt64(keyValue);
                area = areaRepository.Get(id);
                AutoMapper.Mapper.Map<AreaInputDto, Area>(areaInputDto, area);
                area.LastModificationTime = DateTime.Now;
                areaRepository.Update(area);
            }
            else
            {
                AutoMapper.Mapper.Map<AreaInputDto, Area>(areaInputDto, area);
                area.Id = IdWorkerHelper.GenId64();
                area.CreationTime = DateTime.Now;
                area.CreatorUserId = 1;
                areaRepository.Add(area);
            }
        }

        public void DeleteForm(string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            Area area = areaRepository.Get(id);
            area.DeletedMark = true;
            area.DeletionTime = DateTime.Now;
            areaRepository.Update(area);
        }
    }
}
