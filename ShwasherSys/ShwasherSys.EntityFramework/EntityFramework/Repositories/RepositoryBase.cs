using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace ShwasherSys.EntityFramework.Repositories
{
    public abstract class IwbRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<ShwasherDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected IwbRepositoryBase(IDbContextProvider<ShwasherDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class IwbRepositoryBase<TEntity> : IwbRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected IwbRepositoryBase(IDbContextProvider<ShwasherDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
