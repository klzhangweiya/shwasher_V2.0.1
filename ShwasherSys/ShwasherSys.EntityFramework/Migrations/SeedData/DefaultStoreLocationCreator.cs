using System;
using System.Linq;
using Abp.Localization;
using Abp.Timing;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.EntityFramework;
using IwbZero.Setting;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.Migrations.SeedData
{
    public class DefaultStoreLocationCreator
    {
        private readonly ShwasherDbContext _context;

        public DefaultStoreLocationCreator(ShwasherDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            string sql = "TRUNCATE TABLE [dbo].[StoreHouseLocation]";
            _context.Database.ExecuteSqlCommand(sql);
            A();
            B();
            C();
            D();
            E();
            F();
            A1();
            string sql2 = "TRUNCATE TABLE [dbo].[Regions]";
            _context.Database.ExecuteSqlCommand(sql2);
            CreateRegion("0000000000", "全球", "0", 0, "0000000000,");
            CreateRegion("8600000000", "中国", "0000000000", 1, "0000000000,8600000000,");
            CreateRegion("086021", "上海市", "8600000000", 2, "0000000000,8600000000,086021,");
            CreateRegion("0860210200", "金山区", "086021", 3, "0000000000,8600000000,086021,0860210200,");
            CreateRegion("0860210201", "朱泾镇", "0860210200", 4, "0000000000,8600000000,086021,0860210200,0860210201,");
        }

        public void CreateRegion(string regionId,string regionName,string fatherId,int depth,string path,string isLeaf="N",int sort=1, string url="")
        {
            if (_context.Regions.Any(s => s.Id == regionId))
            {
                return;
            }
            _context.Regions.Add(new Regions()
            {
                Id = regionId,
                RegionName = regionName,
                FatherRegionID = fatherId,
                URL = url,
                Depth = depth,
                IsLeaf = isLeaf,
                Path = path,
                Sort = sort,
                TimeLastMod = Clock.Now,
                TimeCreated = Clock.Now,
                UserIDLastMod = "admin",
                IsLock = "N"
            });
            _context.SaveChanges();
        }
        public void A()
        {
            AddStore("A", "1", 1, 8);
            AddStore("A", "1", 2, 8);
            AddStore("A", "1", 3, 8);

            AddStore("A", "2", 1, 6);
            AddStore("A", "2", 2, 6);
            AddStore("A", "2", 3, 6);

            AddStore("A", "3", 1, 10);
            AddStore("A", "3", 2, 10);
            AddStore("A", "3", 3, 10);

            AddStore("A", "4", 1, 9);
            AddStore("A", "4", 2, 9);
            AddStore("A", "4", 3, 9);

            AddStore("A", "5", 1, 6);
            AddStore("A", "5", 2, 6);
            AddStore("A", "5", 3, 6);
        }
        public void B()
        {
            AddStore("B", "1", 1, 12);
            AddStore("B", "1", 2, 12);
            AddStore("B", "1", 3, 12);
            AddStore("B", "1", 4, 12);

            AddStore("B", "2", 1, 12);
            AddStore("B", "2", 2, 12);
            AddStore("B", "2", 3, 12);
            AddStore("B", "2", 4, 12);

            AddStore("B", "3", 1, 12);
            AddStore("B", "3", 2, 12);
            AddStore("B", "3", 3, 12);
            AddStore("B", "3", 4, 12);

            AddStore("B", "4", 1, 14);
            AddStore("B", "4", 2, 14);
            AddStore("B", "4", 3, 14);
            AddStore("B", "4", 4, 14);

            AddStore("B", "5", 1, 18);
            AddStore("B", "5", 2, 18);
            AddStore("B", "5", 3, 18);
            AddStore("B", "5", 4, 18);

            AddStore("B", "6", 1, 10);
            AddStore("B", "6", 2, 10);
            AddStore("B", "6", 3, 10);
            AddStore("B", "6", 4, 10);
        }
        public void C()
        {
            AddStore("C", "1", 1,14);
            AddStore("C", "1", 2,14);
            AddStore("C", "1", 3,14);
            AddStore("C", "1", 4,14);

            AddStore("C", "2", 1,8);
            AddStore("C", "2", 2,8);
            AddStore("C", "2", 3,8);

            AddStore("C", "3", 1,8);
            AddStore("C", "3", 2,8);
            AddStore("C", "3", 3,8);

            AddStore("C", "4", 1,8);
            AddStore("C", "4", 2,8);
            AddStore("C", "4", 3,8);

            AddStore("C", "5", 1,8);
            AddStore("C", "5", 2,8);
            AddStore("C", "5", 3,8);

            AddStore("C", "6", 1,10);
            AddStore("C", "6", 2,10);
            AddStore("C", "6", 3,10);
            AddStore("C", "6", 4,10);

            AddStore("C", "7", 1,4);
            AddStore("C", "7", 2,4);
            AddStore("C", "7", 3,4);
            AddStore("C", "7", 4,4);
        }
        public void D()
        {
            AddStore("D", "1", 1,10);
            AddStore("D", "1", 2,10);
            AddStore("D", "1", 3,10);

            AddStore("D", "2", 1,4);
            AddStore("D", "2", 2,4);
            AddStore("D", "2", 3,4);

            AddStore("D", "3", 1,8);
            AddStore("D", "3", 2,8);
            AddStore("D", "3", 3,8);
            
        }
        public void E()
        {
            AddStore("E", "1", 1,9);
            AddStore("E", "1", 2,9);
            AddStore("E", "1", 3,9);

            AddStore("E", "2", 1,14);
            AddStore("E", "2", 2,14);
            AddStore("E", "2", 3,14);
            
        }
        public void F()
        {
            AddStore("F", "1", 1,14);
            AddStore("F", "1", 2,14);
            AddStore("F", "1", 3,14);

            AddStore("F", "2", 1,2);
            AddStore("F", "2", 2,2);
            AddStore("F", "2", 3,2);
            AddStore("F", "2", 4,2);
            AddStore("F", "2", 5,2);
            AddStore("F", "2", 6,2);

            AddStore("F", "3", 1,4);
            AddStore("F", "3", 2,4);
            AddStore("F", "3", 3,4);
            AddStore("F", "3", 4,4);
            AddStore("F", "3", 5,4);
            AddStore("F", "3", 6,4);

            AddStore("F", "4", 1,4);
            AddStore("F", "4", 2,4);
            AddStore("F", "4", 3,4);
            AddStore("F", "4", 4,4);
            AddStore("F", "4", 5,4);
            AddStore("F", "4", 6,4);
            
        }

        public void A1()
        {
            AddStore("A", "1", 1, 44,21,3);
            AddStore("A", "1", 2, 44,21,3);
            AddStore("A", "1", 3, 44,21,3);
                                       
            AddStore("B", "1", 1, 10, 1,3);
            AddStore("B", "1", 2, 10, 1,3);
            AddStore("B", "1", 3, 10, 1,3);
                                       
            AddStore("C", "1", 1, 10, 1,3);
            AddStore("C", "1", 2, 10, 1,3);
            AddStore("C", "1", 3, 10, 1,3);
                                       
            AddStore("D", "1", 1, 10, 1,3);
            AddStore("D", "1", 2, 10, 1,3);
            AddStore("D", "1", 3, 10, 1,3);
                                       
            AddStore("E", "1", 1, 10, 1,3);
            AddStore("E", "1", 2, 10, 1,3);
            AddStore("E", "1", 3, 10, 1,3);
                                       
            AddStore("F", "1", 1, 17, 1,3);
            AddStore("F", "1", 2, 17, 1,3);
            AddStore("F", "1", 3, 17, 1,3);
                                       
            AddStore("G", "1", 1, 17, 1,3);
            AddStore("G", "1", 2, 17, 1,3);
            AddStore("G", "1", 3, 17, 1,3);

            AddStore("K", "1", 1, 32, 17,3);
            AddStore("K", "1", 2, 32, 17,3);
            AddStore("K", "1", 3, 32, 17,3);
            AddStore("K", "1", 4, 32, 17,3);

            AddStore("P", "1", 1, 3, 1,3);
        }

      

        public void AddStore(string areaCode, string number, int level, int maxSequenceNo, int? minSequenceNo = 1, int? storeNo=1)
        {
            for (int i = minSequenceNo??1; i <= maxSequenceNo; i++)
            {
                AddStoreIfNotExists(areaCode, number, level, i, storeNo??1);
            }
        }

        private void AddStoreIfNotExists(string areaCode, string number, int level, int sequenceNo, int houseId = 1)
        {
            var locationNo = $"{houseId}-{areaCode}-{number}-{level}-{sequenceNo}";
            if (houseId == 1)
            {
                locationNo = $"{areaCode}-{number}-{level}-{sequenceNo}";
            }
           
            if (_context.StoreHouseLocations.Any(s => s.StoreLocationNo == locationNo))
                return;
            _context.StoreHouseLocations.Add(new StoreHouseLocation()
            {
                StoreLocationNo = locationNo,
                StoreAreaCode = areaCode,
                ShelfNumber = number,
                ShelfLevel = level.ToString(),
                SequenceNo = sequenceNo.ToString(),
                StoreHouseId = houseId,
            });
            _context.SaveChanges();
        }
    }
}