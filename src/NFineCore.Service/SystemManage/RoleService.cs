using AutoMapper;
using NFineCore.Support;
using NFineCore.EntityFramework.Dtos.SystemManage;
using NFineCore.EntityFramework.Models.SystemManage;
using NFineCore.Repository.SystemManage;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using Snowflake;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NFineCore.Service.SystemManage
{
    public class RoleService
    {
        RoleRepository roleRepository = new RoleRepository(SharpRepoConfig.sharpRepoConfig, "efCore");
        PermissionRepository permissionRepository = new PermissionRepository(SharpRepoConfig.sharpRepoConfig, "efCore");

        public List<RoleGridDto> GetList()
        {
            var spec = new Specification<Role>();
            spec = new Specification<Role>(p => p.EnabledMark.Equals(true));
            var list = roleRepository.FindAll(spec).ToList();
            return Mapper.Map<List<RoleGridDto>>(list);
        }

        public List<RoleGridDto> GetList(string keyword)
        {
            var spec = new Specification<Role>();
            if (!string.IsNullOrEmpty(keyword))
            {
                spec = new Specification<Role>(p => p.FullName.Contains(keyword) || p.EnCode.Contains(keyword));
            }
            var list = roleRepository.FindAll(spec).ToList();
            return Mapper.Map<List<RoleGridDto>>(list);
        }

        public List<RoleGridDto> GetList(Pagination pagination, string keyword)
        {
            var specification = new Specification<Role>(r => r.DeletedMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                specification = new Specification<Role>(r => r.DeletedMark == false && (r.FullName.Contains(keyword)||r.EnCode.Contains(keyword)));
            }
            var pagingOptions = new PagingOptions<Role, DateTime?>(pagination.page, pagination.rows, x => x.CreationTime, isDescending: true);
            var list = roleRepository.FindAll(specification, pagingOptions).ToList();
            pagination.records = pagingOptions.TotalItems;
            return Mapper.Map<List<RoleGridDto>>(list);
        }

        public RoleOutputDto GetForm(string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            RoleOutputDto roleOutputDto = new RoleOutputDto();
            Role role = roleRepository.Get(id);
            AutoMapper.Mapper.Map<Role, RoleOutputDto>(role, roleOutputDto);
            return roleOutputDto;
        }

        public void SubmitForm(RoleInputDto roleInputDto, string[] resourceIdsArray, string keyValue)
        {
            Role role = new Role();
            long id = Convert.ToInt64(keyValue);
            if (!string.IsNullOrEmpty(keyValue))
            {                
                role = roleRepository.Get(id);
                AutoMapper.Mapper.Map<RoleInputDto, Role>(roleInputDto, role);
                role.LastModificationTime = DateTime.Now;
                roleRepository.Update(role);
            }
            else
            {
                AutoMapper.Mapper.Map<RoleInputDto, Role>(roleInputDto, role);
                role.Id = IdWorkerHelper.GenId64();
                role.CreationTime = DateTime.Now;
                roleRepository.Add(role);
            }
            permissionRepository.Delete(new Specification<Permission>(p => p.ObjectId.Equals(id)));
            List<Permission> permissionList = new List<Permission>();
            foreach (var resourceId in resourceIdsArray)
            {
                Permission permission = new Permission();                
                permission.Id = IdWorkerHelper.GenId64();
                permission.ResourceId = Convert.ToInt64(resourceId);
                permission.ObjectId = id;
                permission.ObjectType = "RolePermission";
                permission.CreationTime = DateTime.Now;
                permission.DeletedMark = false;
                permissionList.Add(permission);
                Debug.WriteLine(permission.Id);
            }
            permissionRepository.Add(permissionList);
        }

        public void DeleteForm(string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            Role role = roleRepository.Get(id);
            role.DeletedMark = true;
            role.DeletionTime = DateTime.Now;
            roleRepository.Update(role);
        }
    }
}
