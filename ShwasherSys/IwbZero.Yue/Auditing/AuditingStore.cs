using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using IwbZero.BaseSysInfo;

namespace IwbZero.Auditing
{
    /// <summary>
    /// Implements <see cref="IAuditingStore"/> to save auditing informations to database.
    /// </summary>
    public class IwbAuditingStore : IAuditingStore, ITransientDependency
    {
        private readonly IRepository<IwbSysLog, long> _auditLogRepository;

        /// <summary>
        /// Creates  a new <see cref="IwbAuditingStore"/>.
        /// </summary>
        public IwbAuditingStore(IRepository<IwbSysLog, long> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public virtual async Task SaveAsync(AuditInfo auditInfo)
        {
            var log =new IwbSysLog().CreateFromAuditInfo(auditInfo);
            if(log!=null)
                await _auditLogRepository.InsertAsync(log);
        }

    }
}
