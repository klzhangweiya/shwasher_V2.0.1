using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.StoreHouses.Dto
{
    [AutoMapTo(typeof(StoreHouse)), AutoMapFrom(typeof(StoreHouse))]
    public class StoreHouseDto : EntityDto<int>
    {
        public string StoreHouseName { get; set; }
        public int? StoreHouseTypeId { get; set; }
        public string StoreHouseTypeName { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string ContactMan { get; set; }
        public string Remark { get; set; }
        public string IsLock { get; set; }
        public DateTime? TimeCreated { get; set; }
        public DateTime? TimeLastMod { get; set; }
        public string UserIDLastMod { get; set; }
        public string StoreHouseNo { get; set; }
    }
}
   
