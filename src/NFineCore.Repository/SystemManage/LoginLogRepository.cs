using NFineCore.EntityFramework.Models.SystemManage;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace NFineCore.Repository.SystemManage
{
    public class LoginLogRepository : ConfigurationBasedRepository<LoginLog, long>
    {
        public LoginLogRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
        {

        }
    }
}
