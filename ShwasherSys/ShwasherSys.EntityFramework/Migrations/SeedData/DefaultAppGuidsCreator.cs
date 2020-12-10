using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.EntityFramework;

namespace ShwasherSys.Migrations.SeedData
{
    public class DefaultAppGuidsCreator
    {
        private readonly ShwasherDbContext _context;

        public DefaultAppGuidsCreator(ShwasherDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DeleteTable("[dbo].[Sys_AppGuids]");
            AddAppGuidIfNotExists((short)AppGuidType.PackageEnterBill,20000);
          

        }

        private void AddAppGuidIfNotExists(short type, int start = 100000, short step = 1)
        {
            if (_context.AppGuids.Any(s => s.IdType == type))
                return;
            _context.AppGuids.Add(new SysAppGuid()
            {
                IdType = type,
                LastId = start,
                Step = step

            });
            _context.SaveChanges();
        }
    }
}
