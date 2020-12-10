using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.StoreHouses.Dto
{
    [AutoMapTo(typeof(StoreHouse))]
    public class StoreHouseCreateDto
    {
        [Required] 
        [StringLength(StoreHouse.StoreHouseNameMaxLength)]
		public string StoreHouseName  { get; set; }
		public int? StoreHouseTypeId  { get; set; }
        [StringLength(StoreHouse.AddressMaxLength)]
		public string Address  { get; set; }
        [StringLength(StoreHouse.TelMaxLength)]
		public string Tel  { get; set; }
        [StringLength(StoreHouse.FaxMaxLength)]
		public string Fax  { get; set; }
        [StringLength(StoreHouse.ContactManMaxLength)]
		public string ContactMan  { get; set; }
        [StringLength(StoreHouse.RemarkMaxLength)]
		public string Remark  { get; set; }
        [StringLength(StoreHouse.IsLockMaxLength)]
		public string IsLock  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(StoreHouse.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
        [StringLength(StoreHouse.StoreHouseNoMaxLength)]
		public string StoreHouseNo  { get; set; }
    }
}
