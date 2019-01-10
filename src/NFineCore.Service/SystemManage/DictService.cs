
using AutoMapper;
using NFineCore.Support;
using NFineCore.EntityFramework.Dtos.SystemManage;
using NFineCore.EntityFramework.Models.SystemManage;
using NFineCore.Repository.SystemManage;
using Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFineCore.Service.SystemManage
{
    public class DictService
    {
        DictRepository dictRepository = new DictRepository(SharpRepoConfig.sharpRepoConfig, "efCore");
        DictItemRepository dictItemRepository = new DictItemRepository(SharpRepoConfig.sharpRepoConfig, "efCore");

        public List<DictGridDto> GetList()
        {
            var list = dictRepository.GetAll().ToList();
            return Mapper.Map<List<DictGridDto>>(list);
        }

        public DictOutputDto GetForm(string keyValue)
        {
            long id = Convert.ToInt64(keyValue);
            DictOutputDto dictOutputDto = new DictOutputDto();
            Dict dict = dictRepository.Get(id);
            AutoMapper.Mapper.Map<Dict, DictOutputDto>(dict, dictOutputDto);
            return dictOutputDto;
        }

        public void SubmitForm(DictInputDto dictInputDto, string keyValue)
        {
            Dict dict = new Dict();
            if (!string.IsNullOrEmpty(keyValue))
            {
                long id = long.Parse(keyValue);
                dict = dictRepository.Get(id);
                AutoMapper.Mapper.Map<DictInputDto, Dict>(dictInputDto, dict);
                dict.LastModificationTime = DateTime.Now;
                dictRepository.Update(dict);
            }
            else
            {
                AutoMapper.Mapper.Map<DictInputDto, Dict>(dictInputDto, dict);
                dict.Id = IdWorkerHelper.GenId64();
                dict.CreationTime = DateTime.Now;
                dictRepository.Add(dict);
            }
        }

        public List<DictItemGridDto> GetDictItemList(string enCode)
        {
            var dict = dictRepository.Find(d => d.EnCode == enCode);
            var dictItems = dictItemRepository.FindAll(i => i.DictId == dict.Id);
            return Mapper.Map<List<DictItemGridDto>>(dictItems);
        }
    }
}
