
using AutoMapper;
using NFineCore.Support;
using NFineCore.EntityFramework;
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
    public class UserService
    {
        UserRepository userRepository = new UserRepository(SharpRepoConfig.sharpRepoConfig,"efCore");
        RoleRepository roleRepository = new RoleRepository(SharpRepoConfig.sharpRepoConfig, "efCore");
        UserRoleRepository userRoleRepository = new UserRoleRepository(SharpRepoConfig.sharpRepoConfig, "efCore");
        OrganizeRepository organizeRepository = new OrganizeRepository(SharpRepoConfig.sharpRepoConfig, "efCore");

        public List<UserGridDto> GetList()
        {
            List<User> list = new List<User>();
            User user = new User();
            user.Id = 1;
            user.UserName = "aaa";
            user.RealName = "阿斯顿发";
            list.Add(user);
            return Mapper.Map<List<UserGridDto>>(list);
        }
        public List<UserGridDto> GetList(Pagination pagination, string keyword)
        {
            var specification = new Specification<User>(u => u.DeletedMark == false);
            var pagingOptions = new PagingOptions<User, DateTime?>(pagination.page, pagination.rows, x => x.CreationTime, isDescending: true);
            if (!string.IsNullOrEmpty(keyword))
            {
                specification = new Specification<User>(u => u.DeletedMark == false && 
                                                            (u.UserName.Contains(keyword) ||
                                                             u.RealName.Contains(keyword) ||
                                                             u.NickName.Contains(keyword) ||
                                                             u.MobilePhone.Contains(keyword) ||
                                                             u.WeChat.Contains(keyword) ||
                                                             u.Email.Contains(keyword)));
            }
            var list = userRepository.FindAll(specification, pagingOptions).ToList();
            pagination.records = pagingOptions.TotalItems;
            return Mapper.Map<List<UserGridDto>>(list);
        }
        public UserOutputDto GetForm(string keyValue) {
            var id = Convert.ToInt64(keyValue);
            UserOutputDto userOutputDto = new UserOutputDto();
            string[] includePath = { "Company", "Department", "Duty", "UserRoles.Role", "UserRoles.User" };
            User user = userRepository.Get(id, includePath);
            AutoMapper.Mapper.Map<User, UserOutputDto>(user, userOutputDto);
            return userOutputDto;
        }
        public void SubmitForm(UserInputDto userInputDto, string[] roleIds, string keyValue)
        {
            User user = new User();
            if (!string.IsNullOrEmpty(keyValue))
            {
                var id = Convert.ToInt64(keyValue);
                string[] includePath = { "Company", "Department", "Duty", "UserRoles", "UserRoles.Role", "UserRoles.User" };
                user = userRepository.Get(id, includePath);
                AutoMapper.Mapper.Map<UserInputDto, User>(userInputDto, user);
                user.LastModificationTime = DateTime.Now;
                userRepository.Update(user);
            }
            else
            {
                AutoMapper.Mapper.Map<UserInputDto, User>(userInputDto, user);
                user.Id = IdWorkerHelper.GenId64();
                user.DeletedMark = false;
                user.CreationTime = DateTime.Now;
                user.CreatorUserId = 1;
                user.SecretKey = Md5.md5(Common.CreateNo(), 16).ToLower();
                user.Password = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userInputDto.Password, 32).ToLower(), user.SecretKey).ToLower(), 32).ToLower();
                userRepository.Add(user);
            }

            #region 删除用户所属角色,再添加用户角色
            userRoleRepository.Delete(new Specification<UserRole>(p => p.UserId == Convert.ToInt64(keyValue)));
            List<UserRole> userRoles = new List<UserRole>();
            for (var i = 0; i < roleIds.Length; i++)
            {
                var userRole = new UserRole();
                userRole.UserId = user.Id;
                userRole.RoleId = Convert.ToInt64(roleIds[i]);
                userRole.DeletedMark = true;
                userRoles.Add(userRole);
            }
            userRoleRepository.Add(userRoles);
            #endregion

        }
        public void UpdateForm(User user)
        {
            user.DeletionTime = DateTime.Now;
            userRepository.Update(user);
        }

        public void DisabledForm(string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            User user = userRepository.Get(id);
            user.EnabledMark = false;
            user.LastModificationTime = DateTime.Now;
            userRepository.Update(user);
        }
        
        public void EnabledForm(string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            User user = userRepository.Get(id);
            user.EnabledMark = true;
            user.LastModificationTime = DateTime.Now;
            userRepository.Update(user);
        }
        public void DeleteForm(string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            User user = userRepository.Get(id);
            user.DeletedMark = true;
            user.DeletionTime = DateTime.Now;
            userRepository.Update(user);
        }

        public UserOutputDto CheckLogin(string username, string password)
        {
            User user = userRepository.Find(t => t.UserName == username);
            if (user != null)
            {
                UserOutputDto userOutputDto = new UserOutputDto();
                if (user.EnabledMark == true)
                {
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), user.SecretKey).ToLower(), 32).ToLower();
                    if (dbPassword == user.Password)
                    {
                        user.LastLoginTime = DateTime.Now;
                        userRepository.Update(user);
                        AutoMapper.Mapper.Map<User, UserOutputDto>(user, userOutputDto);
                        return userOutputDto;
                    }
                    else
                    {
                        throw new Exception("密码不正确，请重新输入。");
                    }
                }
                else
                {
                    throw new Exception("账户被系统锁定,请联系管理员。");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入。");
            }
        }

        public void SubmitResetPassword(string password, string keyValue)
        {
            var id = Convert.ToInt64(keyValue);
            User user = userRepository.Get(id);
            if (user != null)
            {
                user.SecretKey = Md5.md5(Common.CreateNo(), 16).ToLower();
                user.Password = Md5.md5(DESEncrypt.Encrypt(Md5.md5(password, 32).ToLower(), user.SecretKey).ToLower(), 32).ToLower();
                userRepository.Update(user);
            }
        }

        public User VaildUserNameExists(string username)
        {
            User user = userRepository.Find(t => t.UserName == username);
            return user;
        }
    }
}

