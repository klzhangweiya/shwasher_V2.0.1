using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Abp.EntityFramework;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Roles;
using IwbZero.Authorization.Users;
using IwbZero.BaseSysInfo;

namespace IwbZero
{
    public abstract class IwbDbContext<TRole, TUser> : AbpDbContext
        where TRole : IwbSysRole<TUser>,new()
        where TUser : IwbSysUser<TUser>,new()
        //where TFun: IwbSysFunction<TUser>,new()
    {
        //TODO: Define an IDbSet for each Entity...

        public virtual IDbSet<TUser> Users { get; set; }
        public virtual IDbSet<UserLogin> UserLogins { get; set; }
        public virtual IDbSet<UserLoginAttempt> UserLoginLogs { get; set; }
        public virtual IDbSet<TRole> Roles { get; set; }
        public virtual IDbSet<SysUserRole> UserRoles { get; set; }
        public virtual IDbSet<SysPermission> Permissions { get; set; }
        //public virtual IDbSet<TFun> Functions { get; set; }
        //public virtual IDbSet<IwbSysSetting<TUser>> Settings { get; set; }
        //public virtual IDbSet<IwbSysState<TUser>> SysStates { get; set; }



        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */

        /// <summary>
        /// Default constructor.
        /// Do not directly instantiate this class. Instead, use dependency injection!
        /// </summary>
        protected IwbDbContext()
        {

        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or a name in connection strings in configuration file</param>
        protected IwbDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        protected IwbDbContext(DbCompiledModel model)
            : base(model)
        {

        }

        /// <summary>
        /// This constructor can be used for unit tests.
        /// </summary>
        protected IwbDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {

        }

        protected IwbDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {

        }

        protected IwbDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected IwbDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region IwbSysLog.Set_MaxLengths

            modelBuilder.Entity<IwbSysLog>()
                .Property(e => e.ServiceName)
                .HasMaxLength(IwbSysLog.MaxServiceNameLength);

            modelBuilder.Entity<IwbSysLog>()
                .Property(e => e.MethodName)
                .HasMaxLength(IwbSysLog.MaxMethodNameLength);

            modelBuilder.Entity<IwbSysLog>()
                .Property(e => e.Parameters)
                .HasMaxLength(IwbSysLog.MaxParametersLength);

            modelBuilder.Entity<IwbSysLog>()
                .Property(e => e.ClientIpAddress)
                .HasMaxLength(IwbSysLog.MaxClientIpAddressLength);

            modelBuilder.Entity<IwbSysLog>()
                .Property(e => e.ClientName)
                .HasMaxLength(IwbSysLog.MaxClientNameLength);

            modelBuilder.Entity<IwbSysLog>()
                .Property(e => e.BrowserInfo)
                .HasMaxLength(IwbSysLog.MaxBrowserInfoLength);

            modelBuilder.Entity<IwbSysLog>()
                .Property(e => e.Exception)
                .HasMaxLength(IwbSysLog.MaxExceptionLength);

            modelBuilder.Entity<IwbSysLog>()
                .Property(e => e.CustomData)
                .HasMaxLength(IwbSysLog.MaxCustomDataLength);

            #endregion
        }
    }
}
