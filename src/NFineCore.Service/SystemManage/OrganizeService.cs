
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
    public class OrganizeService
    {
        OrganizeRepository organizeRepository = new OrganizeRepository(SharpRepoConfig.sharpRepoConfig, "efCore");

        public List<OrganizeGridDto> GetList()
        {
            var specification = new Specification<Organize>(u => u.DeletedMark == false);
            var sortingOtopns = new SortingOptions<Organize, int?>(x => x.SortCode, isDescending: true);
            var list = organizeRepository.FindAll(specification, sortingOtopns).ToList();
            return Mapper.Map<List<OrganizeGridDto>>(list);
        }

        public OrganizeOutputDto GetForm(string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            OrganizeOutputDto organizeOutputDto = new OrganizeOutputDto();
            Organize organize = organizeRepository.Get(id);
            AutoMapper.Mapper.Map<Organize, OrganizeOutputDto>(organize, organizeOutputDto);
            return organizeOutputDto;
        }

        public void SubmitForm(OrganizeInputDto organizeInputDto, string keyValue)
        {
            Organize organize = new Organize();
            if (!string.IsNullOrEmpty(keyValue))
            {
                var id = Convert.ToInt64(keyValue);
                organize = organizeRepository.Get(id);
                AutoMapper.Mapper.Map<OrganizeInputDto, Organize>(organizeInputDto, organize);
                organize.LastModificationTime = DateTime.Now;
                organizeRepository.Update(organize);
            }
            else
            {
                AutoMapper.Mapper.Map<OrganizeInputDto, Organize>(organizeInputDto, organize);
                organize.Id = IdWorkerHelper.GenId64();
                organize.DeletedMark = false;
                organize.CreationTime = DateTime.Now;
                organizeRepository.Add(organize);
            }
        }

        public void DeleteForm(string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            Organize organize = organizeRepository.Get(id);
            organize.DeletedMark = true;
            organize.DeletionTime = DateTime.Now;
            organizeRepository.Update(organize);
        }
    }
}
