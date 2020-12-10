using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.CustomerInfo;

namespace ShwasherSys.CustomerInfo.Dto
{
    [AutoMapTo(typeof(CustomerSend))]
    public class CustomerSendCreateDto
    {
        [Required] 
        [StringLength(CustomerSend.CustomerIdMaxLength)]
		public string CustomerId  { get; set; }
        [Required] 
        [StringLength(CustomerSend.CustomerSendNameMaxLength)]
		public string CustomerSendName  { get; set; }
        [Required] 
        [StringLength(CustomerSend.SendAdressMaxLength)]
		public string SendAdress  { get; set; }
        [StringLength(CustomerSend.LinkManMaxLength)]
		public string LinkMan  { get; set; }
        [StringLength(CustomerSend.TelephoneMaxLength)]
		public string Telephone  { get; set; }
        [StringLength(CustomerSend.ZipMaxLength)]
		public string Zip  { get; set; }
        [StringLength(CustomerSend.EmailMaxLength)]
		public string Email  { get; set; }
        [StringLength(CustomerSend.MobileMaxLength)]
		public string Mobile  { get; set; }
        [StringLength(CustomerSend.FaxMaxLength)]
		public string Fax  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(CustomerSend.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
        
        [StringLength(CustomerSend.IsLockMaxLength)]
		public string IsLock  { get; set; }
    }
}
