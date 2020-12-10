using System;
using System.Linq;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BasicInfo;
using ShwasherSys.EntityFramework;

namespace ShwasherSys.Migrations.SeedData
{
    public class DefaultDataCreator
    {
        private readonly ShwasherDbContext _context;

        public DefaultDataCreator(ShwasherDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateQualityLabels();
            CreateScrapTypes();
            CreateFixedTypes();
        }

        private void CreateQualityLabels()
        {
            CreateQualityLabel("发货问题");
            CreateQualityLabel("半成品问题");
            CreateQualityLabel("包装问题");
            CreateQualityLabel("车间问题");
            CreateQualityLabel("市场部问题");
            CreateQualityLabel("销售部问题");
            CreateQualityLabel("生产部问题");
        }

        private void CreateScrapTypes()
        {
            CreateScrapType("生产报废");
            CreateScrapType("生产损耗");
            CreateScrapType("客户退货");
      
        }
        private void CreateFixedTypes()
        {
            CreateFixedType("生产设备");
            CreateFixedType("包装设备");
            CreateFixedType("特种设备");
            CreateFixedType("大型加工机械");
      
        }


        private void CreateQualityLabel(string name)
        {
            if (_context.QualityIssueLabelInfo.Any(a=>a.Name==name))
            {
                return;
            }

            _context.QualityIssueLabelInfo.Add(new QualityIssueLabel()
            {
                 Id= Guid.NewGuid().ToString("N"),
                Name = name,
                Description = name
            });
            _context.SaveChanges();
        } 
      
        private void CreateScrapType(string name)
        {
            if (_context.ScrapTypeInfo.Any(a=>a.Name==name))
            {
                return;
            }

            _context.ScrapTypeInfo.Add(new ScrapType()
            {
                 Id= Guid.NewGuid().ToString("N"),
                Name = name,
                Description = name
            });
            _context.SaveChanges();
        } 
      
        private void CreateFixedType(string name)
        {
            if (_context.FixedAssetTypeInfo.Any(a=>a.Name==name))
            {
                return;
            }

            _context.FixedAssetTypeInfo.Add(new FixedAssetType()
            {
                 Id= Guid.NewGuid().ToString("N"),
                Name = name,
                Description = name
            });
            _context.SaveChanges();
        } 
      
    }
}