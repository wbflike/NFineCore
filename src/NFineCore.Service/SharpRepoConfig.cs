using Microsoft.Extensions.Configuration;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFineCore.Service
{
    public static class SharpRepoConfig
    {
        public static ISharpRepositoryConfiguration sharpRepoConfig;

        static SharpRepoConfig()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            var sectionName = "sharpRepository";
            //var repositoryName = "efCore";
            IConfigurationSection sharpRepoSection = config.GetSection(sectionName);
            sharpRepoConfig = RepositoryFactory.BuildSharpRepositoryConfiguation(sharpRepoSection);
        }
    }
}
