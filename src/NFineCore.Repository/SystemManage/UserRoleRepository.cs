using NFineCore.EntityFramework.Models.SystemManage;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace NFineCore.Repository.SystemManage
{
    public class UserRoleRepository : ConfigurationBasedRepository<UserRole, long>
    {
        public UserRoleRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
        {

        }
    }
}
