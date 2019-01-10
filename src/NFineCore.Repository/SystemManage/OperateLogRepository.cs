using NFineCore.EntityFramework.Models.SystemManage;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace NFineCore.Repository.SystemManage
{
    public class OperateLogRepository : ConfigurationBasedRepository<OperateLog, long>
    {
        public OperateLogRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
        {

        }
    }
}
