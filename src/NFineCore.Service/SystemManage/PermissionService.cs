using AutoMapper;
using NFineCore.Support;
using NFineCore.EntityFramework.Dtos.SystemManage;
using NFineCore.EntityFramework.Models.SystemManage;
using NFineCore.Repository.SystemManage;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFineCore.Service.SystemManage
{
    public class PermissionService
    {
        PermissionRepository permissionRepository = new PermissionRepository(SharpRepoConfig.sharpRepoConfig, "efCore");
        UserRepository userRepository = new UserRepository(SharpRepoConfig.sharpRepoConfig, "efCore");

        public List<ResourceGridDto> GetPermsResList(long userId)
        {
            var resourceList = new List<Resource>();
            string[] includePath = { "Company", "Department", "Duty", "UserRoles", "UserRoles.Role", "UserRoles.User" };
            User user = userRepository.Get(userId, includePath);

            //添加角色权限资源
            var rolePermsResList = GetRolePermsResList(user.UserRoles);
            resourceList.AddRange(rolePermsResList);

            resourceList = resourceList.Distinct().ToList();
            return Mapper.Map<List<ResourceGridDto>>(resourceList);
        }

        public List<Resource> GetRolePermsResList(List<UserRole> userRoles)
        {
            List<Resource> rolePermsResList = new List<Resource>();
            string roleIds = string.Empty;
            string roleNames = string.Empty;
            if (userRoles.Count > 0)
            {
                foreach (UserRole ur in userRoles)
                {
                    roleIds += ur.Role.Id + ",";
                    roleNames += ur.Role.FullName + ",";
                }
                roleIds = roleIds.TrimEnd(',');
                roleNames = roleNames.TrimEnd(',');
            }
            long[] objectIds = Array.ConvertAll<string, long>(roleIds.Split(','), delegate (string s) { return long.Parse(s); });
            var roleSpecification = new Specification<Permission>(p => objectIds.Contains(p.ObjectId));
            roleSpecification.FetchStrategy.Include(obj => obj.Resource);
            var rolePermissions = permissionRepository.FindAll(roleSpecification);
            foreach (Permission p in rolePermissions)
            {
                rolePermsResList.Add(p.Resource);
            }
            return rolePermsResList;
        }

        public List<PermissionGridDto> GetRolePermissionList(long roleId)
        {
            var specification = new Specification<Permission>(p => p.ObjectId.Equals(roleId) && p.ObjectType.Equals("RolePermission"));
            var list = permissionRepository.FindAll(specification).ToList();
            return Mapper.Map<List<PermissionGridDto>>(list);
        }

        public List<PermissionGridDto> GetUserPermissionList(long userId)
        {
            var specification = new Specification<Permission>(p => p.ObjectId.Equals(userId) && p.ObjectType.Equals("UserPermission"));
            var list = permissionRepository.FindAll(specification).ToList();
            return Mapper.Map<List<PermissionGridDto>>(list);
        }
    }
}
