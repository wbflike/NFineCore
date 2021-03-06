﻿using NFineCore.EntityFramework.Models.SystemManage;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace NFineCore.Repository.SystemManage
{
    public class AreaRepository : ConfigurationBasedRepository<Area, long>
    {
        public AreaRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
        {

        }
    }
}
