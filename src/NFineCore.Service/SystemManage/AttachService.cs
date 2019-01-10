
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
using System.Threading;

namespace NFineCore.Service.SystemManage
{
    public class AttachService
    {
        AttachRepository attachRepository = new AttachRepository(SharpRepoConfig.sharpRepoConfig, "efCore");
        //public List<AreaGridDto> GetList()
        //{
        //    var specification = new Specification<Area>(u => u.DeletedMark == false);
        //    var sortingOtopns = new SortingOptions<Area, int?>(x => x.SortCode, isDescending: false);
        //    var list = areaRepository.FindAll(specification, sortingOtopns).ToList();
        //    return Mapper.Map<List<AreaGridDto>>(list);
        //}

        //public AreaOutputDto GetForm(string keyValue)
        //{
        //    var id = Convert.ToInt64(keyValue);
        //    AreaOutputDto areaOutputDto = new AreaOutputDto();
        //    Area area = areaRepository.Get(id);
        //    AutoMapper.Mapper.Map<Area, AreaOutputDto>(area, areaOutputDto);
        //    return areaOutputDto;
        //}

        public void SubmitForm(AttachInputDto attachInputDto, string keyValue)
        {
            Attach attach = new Attach();
            if (!string.IsNullOrEmpty(keyValue))
            {
                //var id = Convert.ToInt64(keyValue);
                //area = areaRepository.Get(id);
                //AutoMapper.Mapper.Map<AreaInputDto, Area>(areaInputDto, area);
                //area.LastModificationTime = DateTime.Now;
                //areaRepository.Update(area);
            }
            else
            {
                try
                {
                    AutoMapper.Mapper.Map<AttachInputDto, Attach>(attachInputDto, attach);
                    attach.Id = IdWorkerHelper.GenId64();
                    attach.CreationTime = DateTime.Now;
                    attach.CreatorUserId = 1;
                    attachRepository.Add(attach);
                }
                catch
                {
                    AutoMapper.Mapper.Map<AttachInputDto, Attach>(attachInputDto, attach);
                    attach.Id = IdWorkerHelper.GenId64();
                    attach.CreationTime = DateTime.Now;
                    attach.CreatorUserId = 1;
                    attachRepository.Add(attach);
                }
            }
        }

        //public void DeleteForm(string keyValue)
        //{
        //    var id = Convert.ToInt64(keyValue);
        //    Area area = areaRepository.Get(id);
        //    area.DeletedMark = true;
        //    area.DeletionTime = DateTime.Now;
        //    areaRepository.Update(area);
        //}
    }
}
