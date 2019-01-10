using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NFineCore.EntityFramework.Models.SystemManage;

namespace NFineCore.EntityFramework.Maps.SystemManage
{
    public class UserRoleMap:IEntityTypeConfiguration<UserRole>
    {
        /// <summary>
        /// PassageCategories FluentAPI配置
        /// 
        /// 添加复合主键、配置多对多关系
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            //添加复合主键
            builder.HasKey(t => new { t.UserId, t.RoleId });

            ///<summary>
            ///
            /// 配置Passage与PassageCategories的一对多关系
            /// 
            /// EFCore中,新增默认级联模式为ClientSetNull
            /// 
            /// 依赖实体的外键会被设置为空，同时删除操作不会作用到依赖的实体上，依赖实体保持不变，同下
            /// 
            /// </summary>

            //配置Passage与PassageCategories的一对多关系
            builder.HasOne(t => t.User).WithMany(p => p.UserRoles).HasForeignKey(t => t.UserId);

            //配置Category与PassageCategories的一对多关系
            builder.HasOne(t => t.Role).WithMany(p => p.UserRoles).HasForeignKey(t => t.RoleId);
        }
    }
}
